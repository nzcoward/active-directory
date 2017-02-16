using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Arc.ActiveDirectory
{
    public class DirectoryEntryWrapper : IDisposable
    {
        private static object _lock = new object();
        private static int _count;

        private readonly DirectoryEntry _directoryEntry;

        public event EventHandler Disposed;

        public DirectoryEntryWrapper(DirectoryEntry directoryEntry)
        {
            _directoryEntry = directoryEntry;
        }

        #region Delegating Members

        public object GetLifetimeService()
        {
            return _directoryEntry.GetLifetimeService();
        }

        public object InitializeLifetimeService()
        {
            return _directoryEntry.InitializeLifetimeService();
        }

        public ObjRef CreateObjRef(Type requestedType)
        {
            return _directoryEntry.CreateObjRef(requestedType);
        }

        public ISite Site
        {
            get { return _directoryEntry.Site; }
            set { _directoryEntry.Site = value; }
        }

        public IContainer Container
        {
            get { return _directoryEntry.Container; }
        }

        public void Close()
        {
            _directoryEntry.Close();
        }

        public void CommitChanges()
        {
            _directoryEntry.CommitChanges();
        }

        public DirectoryEntry CopyTo(DirectoryEntry newParent)
        {
            return _directoryEntry.CopyTo(newParent);
        }

        public DirectoryEntry CopyTo(DirectoryEntry newParent, string newName)
        {
            return _directoryEntry.CopyTo(newParent, newName);
        }

        public void DeleteTree()
        {
            _directoryEntry.DeleteTree();
        }

        public object Invoke(string methodName, params object[] args)
        {
            return _directoryEntry.Invoke(methodName, args);
        }

        public object InvokeGet(string propertyName)
        {
            return _directoryEntry.InvokeGet(propertyName);
        }

        public void InvokeSet(string propertyName, params object[] args)
        {
            _directoryEntry.InvokeSet(propertyName, args);
        }

        public void MoveTo(DirectoryEntry newParent)
        {
            _directoryEntry.MoveTo(newParent);
        }

        public void MoveTo(DirectoryEntry newParent, string newName)
        {
            _directoryEntry.MoveTo(newParent, newName);
        }

        public void RefreshCache()
        {
            _directoryEntry.RefreshCache();
        }

        public void RefreshCache(string[] propertyNames)
        {
            _directoryEntry.RefreshCache(propertyNames);
        }

        public void Rename(string newName)
        {
            _directoryEntry.Rename(newName);
        }

        public AuthenticationTypes AuthenticationType
        {
            get { return _directoryEntry.AuthenticationType; }
            set { _directoryEntry.AuthenticationType = value; }
        }

        public DirectoryEntries Children
        {
            get { return _directoryEntry.Children; }
        }

        public Guid Guid
        {
            get { return _directoryEntry.Guid; }
        }

        public ActiveDirectorySecurity ObjectSecurity
        {
            get { return _directoryEntry.ObjectSecurity; }
            set { _directoryEntry.ObjectSecurity = value; }
        }

        public string Name
        {
            get { return _directoryEntry.Name; }
        }

        public string NativeGuid
        {
            get { return _directoryEntry.NativeGuid; }
        }

        public object NativeObject
        {
            get { return _directoryEntry.NativeObject; }
        }

        public DirectoryEntry Parent
        {
            get { return _directoryEntry.Parent; }
        }

        public string Password
        {
            set { _directoryEntry.Password = value; }
        }

        public string Path
        {
            get { return _directoryEntry.Path; }
            set { _directoryEntry.Path = value; }
        }

        public PropertyCollection Properties
        {
            get { return _directoryEntry.Properties; }
        }

        public string SchemaClassName
        {
            get { return _directoryEntry.SchemaClassName; }
        }

        public DirectoryEntry SchemaEntry
        {
            get { return _directoryEntry.SchemaEntry; }
        }

        public bool UsePropertyCache
        {
            get { return _directoryEntry.UsePropertyCache; }
            set { _directoryEntry.UsePropertyCache = value; }
        }

        public string Username
        {
            get { return _directoryEntry.Username; }
            set { _directoryEntry.Username = value; }
        }

        public DirectoryEntryConfiguration Options
        {
            get { return _directoryEntry.Options; }
        }

        #endregion

        public void Enlist()
        {
            lock (_lock)
                _count++;
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _count--;
                if (_count > 0)
                    return;
            }

            _directoryEntry.Dispose();
            OnDisposed();
        }

        private void OnDisposed()
        {
            var disposed = this.Disposed;
            if (disposed == null)
                return;

            disposed(this, EventArgs.Empty);
        }
    }
}
