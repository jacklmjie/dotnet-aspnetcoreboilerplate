using DynamicPlugins.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DynamicPlugins.Infrastructure
{
    /// <summary>
    /// 用来静态缓存相同的插件，IReferenceLoader不需要重复加载相同的插件引用
    /// </summary>
    public interface IReferenceContainer
    {
        List<CachedReferenceItemKey> GetAll();

        bool Exist(string name, string version);

        void SaveStream(string name, string version, Stream stream);

        Stream GetStream(string name, string version);
    }

    public class DefaultReferenceContainer : IReferenceContainer
    {
        private static Dictionary<CachedReferenceItemKey, Stream> _cachedReferences = new Dictionary<CachedReferenceItemKey, Stream>();


        public List<CachedReferenceItemKey> GetAll()
        {
            return _cachedReferences.Keys.ToList();
        }

        public bool Exist(string name, string version)
        {
            return _cachedReferences.Keys.Any(p => p.ReferenceName == name
                && p.Version == version);
        }

        public void SaveStream(string name, string version, Stream stream)
        {
            if (Exist(name, version))
            {
                return;
            }


            _cachedReferences.Add(new CachedReferenceItemKey { ReferenceName = name, Version = version }, stream);
        }

        public Stream GetStream(string name, string version)
        {
            var key = _cachedReferences.Keys.FirstOrDefault(p => p.ReferenceName == name
                && p.Version == version);

            if (key != null)
            {
                _cachedReferences[key].Position = 0;
                return _cachedReferences[key];
            }

            return null;
        }
    }
}
