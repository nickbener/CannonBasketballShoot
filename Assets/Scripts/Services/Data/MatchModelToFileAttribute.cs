using System;

namespace Services.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MatchModelToFileAttribute : Attribute
    {
        public string FileName { get; }

        public MatchModelToFileAttribute(string fileName)
        {
            FileName = fileName;
        }
    }
}