using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ResourceSystem;
using Services.Data.Crypto;
using UnityEngine;
using UnityEngine.Assertions;
using Application = UnityEngine.Device.Application;

namespace Services.Data
{
    public class DataService
    {
        private const string ModelsPath = "Data/Models";
        private const string SharpAssemblyName = "Assembly-CSharp";

        private Assembly _sharpAssembly;
        private DirectoryInfo _modelsDirectoryInfo;
        
        private readonly Dictionary<Type, BaseDataModel> _dataModels = new Dictionary<Type, BaseDataModel>();

        private IStreamCryptoService _cryptoService;
        
        public DataService(IStreamCryptoService cryptoService = null)
        {
            _cryptoService = cryptoService;
            
            InitializeModelsDirectory();
            LoadDataModels();
            
            Debug.Log("[DataService] Initialized");
        }

        public TModel GetModel<TModel>() where TModel : BaseDataModel
        {
            Type modelType = typeof(TModel);
            
            if (_dataModels.TryGetValue(modelType, out BaseDataModel model))
            {
                return model as TModel;
            }
            else
            {
                throw new InvalidOperationException(
                    "[DataService] Data model of type: " + modelType + " not present in the collection");
            }
        }
        
        public void ClearAllDataModels()
        {
            foreach (var pair in _dataModels)
            {
                if (IsDataModelType(pair.Key, out MatchModelToFileAttribute modelToFileAttribute))
                {
                    string fullFileName = GetFullFileName(modelToFileAttribute);
                    File.Delete(fullFileName);
                }
            }
        }
        
        private void InitializeModelsDirectory()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, ModelsPath);
            _modelsDirectoryInfo = new DirectoryInfo(fullPath);
        
            if (!_modelsDirectoryInfo.Exists)
            {
                _modelsDirectoryInfo.Create();
            }
            
            Debug.Log("[DataService] Target directory: " + _modelsDirectoryInfo.FullName);
        }

        private void LoadDataModels()
        {
            _sharpAssembly = AppDomain.CurrentDomain
                .GetAssemblies().First(assembly => assembly.GetName().Name.Equals(SharpAssemblyName));
            
            foreach(Type type in _sharpAssembly.GetTypes())
            {
                if (IsDataModelType(type, out MatchModelToFileAttribute modelToFileAttribute))
                {
                    string fullFileName = GetFullFileName(modelToFileAttribute);
                    BaseDataModel dataModelInstance;
                
                    if (!File.Exists(fullFileName))
                    {
                        dataModelInstance = (BaseDataModel) Activator.CreateInstance(type);
                        Debug.Log("[DataService] " + modelToFileAttribute.FileName + " not found. Default model created");
                        File.Create(fullFileName);
                        Debug.Log("[DataService] file created");
                    }
                    else
                    {
                        try
                        {
                            string encryptedNotation = File.ReadAllText(fullFileName);

                            if (string.IsNullOrEmpty(encryptedNotation) || string.IsNullOrWhiteSpace(encryptedNotation))
                            {
                                dataModelInstance = (BaseDataModel) Activator.CreateInstance(type);
                                Debug.Log("[DataService] " + modelToFileAttribute.FileName + " is empty. Default model created");
                            }
                            else
                            {
                                string jsonNotation = _cryptoService != null
                                    ? _cryptoService.Decrypt(encryptedNotation)
                                    : encryptedNotation;
                            
                                dataModelInstance = (BaseDataModel) JsonConvert.DeserializeObject(jsonNotation, type);
                                Assert.IsNotNull(dataModelInstance);
                            }
                        }
                        catch (Exception ex)
                        {
                            dataModelInstance = (BaseDataModel) Activator.CreateInstance(type);
                            Exception loggedException = 
                                new Exception("[DataService] Deserialization of " + modelToFileAttribute.FileName + " is failed. Default model created");
                            Debug.LogException(loggedException);
                            
                            File.Create(fullFileName);
                            Debug.Log("[DataService] file created");
                        }
                    }
                    
                    dataModelInstance.OnDemandSave += OnDemandSave;
                    _dataModels.Add(type, dataModelInstance);
                }

            }
        }
        
        private static bool IsDataModelType(Type type, out MatchModelToFileAttribute modelToFileAttribute)
        {
            if (!type.IsSubclassOf(typeof(BaseDataModel)))
            {
                modelToFileAttribute = null;
                return false;
            }
            
            object[] attributes = type.GetCustomAttributes(typeof(MatchModelToFileAttribute), false);

            if (attributes.Length == 0)
            {
                modelToFileAttribute = null;
                return false;
            }

            modelToFileAttribute = attributes[0] as MatchModelToFileAttribute;

            return modelToFileAttribute != null;
        }

        private string GetFullFileName(MatchModelToFileAttribute modelToFileAttribute)
        {
            return Path.Combine(_modelsDirectoryInfo.FullName, modelToFileAttribute.FileName);
        }

        private void OnDemandSave(BaseDataModel dataModel)
        {
            Task.Run(() => { Save(dataModel); });
        }

        private void Save(BaseDataModel dataModel)
        {
            if (IsDataModelType(dataModel.GetType(), out MatchModelToFileAttribute modelToFileAttribute))
            {
                string fullFileName = GetFullFileName(modelToFileAttribute);
                string jsonNotation = JsonConvert.SerializeObject(dataModel);
                string encryptedNotation = _cryptoService != null ? _cryptoService.Encrypt(jsonNotation) : jsonNotation;
                
                lock (dataModel)
                {
                    File.WriteAllText(fullFileName, encryptedNotation);
                }
            }
        }
        
    }
    
}