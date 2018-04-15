﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Rocket.API.Commands;
using Rocket.API.Configuration;
using Rocket.API.DependencyInjection;
using Rocket.API.Permissions;
using Rocket.Core.ServiceProxies;

namespace Rocket.Core.Permissions
{
    public class PermissionProviderProxy : ServiceProxy<IPermissionProvider>, IPermissionProvider
    {
        public PermissionProviderProxy(IDependencyContainer container) : base(container) { }
        

        public bool SupportsCaller(ICommandCaller caller)
        {
            return ProxiedProviders.Any(c => c.SupportsCaller(caller));
        }

        public bool SupportsGroup(IPermissionGroup group)
        {
            return ProxiedProviders.Any(c => c.SupportsGroup(group));
        }

        public PermissionResult HasPermission(IPermissionGroup @group, string permission)
        {
            GuardGroup(group);

            foreach (var provider in ProxiedProviders.Where(c => c.SupportsGroup(group)))
            {
                PermissionResult result = provider.HasPermission(group, permission);
                if(result == PermissionResult.Default)
                    continue;

                return result;
            }

            return PermissionResult.Default;
        }

        public PermissionResult HasPermission(ICommandCaller caller, string permission)
        {
            GuardCaller(caller);

            foreach (var provider in ProxiedProviders.Where(c => c.SupportsCaller(caller)))
            {
                PermissionResult result = provider.HasPermission(caller, permission);
                if (result == PermissionResult.Default)
                    continue;

                return result;
            }

            return PermissionResult.Default;
        }

        public PermissionResult HasAllPermissions(IPermissionGroup @group, params string[] permissions)
        {
            GuardGroup(group);

            foreach (var provider in ProxiedProviders.Where(c => c.SupportsGroup(group)))
            {
                PermissionResult result = provider.HasAllPermissions(@group, permissions);
                if (result == PermissionResult.Default)
                    continue;

                return result;
            }

            return PermissionResult.Default;
        }

        public PermissionResult HasAllPermissions(ICommandCaller caller, params string[] permissions)
        {
            GuardCaller(caller);

            foreach (var provider in ProxiedProviders.Where(c => c.SupportsCaller(caller)))
            {
                PermissionResult result = provider.HasAllPermissions(caller, permissions);
                if (result == PermissionResult.Default)
                    continue;

                return result;
            }

            return PermissionResult.Default;
        }

        public PermissionResult HasAnyPermissions(IPermissionGroup @group, params string[] permissions)
        {
            GuardGroup(group);

            foreach (var provider in ProxiedProviders.Where(c => c.SupportsGroup(group)))
            {
                PermissionResult result = provider.HasAnyPermissions(@group, permissions);
                if (result == PermissionResult.Default)
                    continue;

                return result;
            }

            return PermissionResult.Default;
        }

        public PermissionResult HasAnyPermissions(ICommandCaller caller, params string[] permissions)
        {
            GuardCaller(caller);

            foreach (var provider in ProxiedProviders.Where(c => c.SupportsCaller(caller)))
            {
                PermissionResult result = provider.HasAnyPermissions(caller, permissions);
                if (result == PermissionResult.Default)
                    continue;

                return result;
            }

            return PermissionResult.Default;
        }

        public bool AddPermission(IPermissionGroup @group, string permission)
        {
            throw new NotSupportedException("Adding permissions from proxy is not supported.");
        }

        public bool AddDeniedPermission(IPermissionGroup @group, string permission)
        {
            throw new NotSupportedException("Adding inverted permissions from proxy is not supported.");
        }

        public bool AddPermission(ICommandCaller caller, string permission)
        {
            throw new NotSupportedException("Adding permissions from proxy is not supported.");
        }

        public bool AddDeniedPermission(ICommandCaller caller, string permission)
        {
            throw new NotSupportedException("Adding inverted permissions from proxy is not supported.");
        }

        public bool RemovePermission(IPermissionGroup @group, string permission)
        {
            throw new NotSupportedException("Removing permissions from proxy is not supported.");
        }

        public bool RemoveDeniedPermission(IPermissionGroup @group, string permission)
        {
            throw new NotSupportedException("Removing inverted permissions from proxy is not supported.");
        }

        public bool RemovePermission(ICommandCaller caller, string permission)
        {
            throw new NotSupportedException("Removing permissions from proxy is not supported.");
        }

        public bool RemoveDeniedPermission(ICommandCaller @group, string permission)
        {
            throw new NotSupportedException("Removing denied permissions from proxy is not supported.");
        }

        public IPermissionGroup GetPrimaryGroup(ICommandCaller caller)
        {
            throw new NotSupportedException("Getting primary group from proxy is not supported.");
        }

        public IPermissionGroup GetGroup(string id)
        {
            throw new NotSupportedException("Getting a group via id from proxy is not supported.");
        }

        public IEnumerable<IPermissionGroup> GetGroups(ICommandCaller caller)
        {
            GuardCaller(caller);

            return ProxiedProviders.SelectMany(c => c.GetGroups(caller));
        }

        public IEnumerable<IPermissionGroup> GetGroups()
        {
            return ProxiedProviders.SelectMany(c => c.GetGroups());
        }

        public void UpdateGroup(IPermissionGroup @group)
        {
            throw new NotSupportedException("Updating groups from proxy is not supported.");
        }

        public void AddGroup(ICommandCaller caller, IPermissionGroup @group)
        {
            throw new NotSupportedException("Adding groups from proxy is not supported.");
        }

        public bool RemoveGroup(ICommandCaller caller, IPermissionGroup @group)
        {
            throw new NotSupportedException("Removing groups from proxy is not supported.");
        }

        public void CreateGroup(IPermissionGroup @group)
        {
            throw new NotSupportedException("Creating groups from proxy is not supported.");
        }

        public void DeleteGroup(IPermissionGroup @group)
        {
            throw new NotSupportedException("Deleting groups from proxy is not supported.");
        }

        public void Load(IConfigurationElement groups, IConfigurationElement players)
        {
            throw new NotSupportedException("Loading from proxy is not supported.");
        }

        public void Reload()
        {
            ProxiedProviders.ForEach(c => c.Reload());
        }

        public void Save()
        {
            ProxiedProviders.ForEach(c => c.Save());
        }

        private void GuardCaller(ICommandCaller caller)
        {
            if(!SupportsCaller(caller))
                throw new NotSupportedException(caller.GetType().FullName + " is not supported!");
        }

        private void GuardGroup(IPermissionGroup group)
        {
            if (!SupportsGroup(group))
                throw new NotSupportedException(group.GetType().FullName + " is not supported!");
        }
    }
}