using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Pooyan.test.infrastructure.StatesBase;

namespace Pooyan.test.project.StateManagement.StateManagerExtensions
{
    public static class StateBaseExtensions
    {
        public static event Action OnChange;
        private static IServiceProvider _serviceProvider;
        private static ILocalStorageService _localStorageService { get; set; }
        private static ISyncLocalStorageService _syncLocalStorageService;
        public static bool IsFromMaui;
        public static bool IsFreezeException;
        public static bool HandleProblems;
        internal static void ConfigLocalStorage(IServiceProvider serviceProvider)
        {
            _localStorageService = serviceProvider.GetRequiredService<ILocalStorageService>();
        }
        internal static void ConfigLocalStorage(ILocalStorageService localStorage)
        {
            _localStorageService = localStorage;
        }
        public static StateBase<TEntity> GetItem<TEntity>(this StateBase<TEntity> entity) where TEntity : class
        {
            if (entity?.CurrentItem != null)
                return entity;
            if (IsFromMaui)
                return GetItemAsync(entity).Result;

            TEntity item = _syncLocalStorageService.GetItem<TEntity>(entity?.TypeName ?? typeof(TEntity).Name);
            entity.CurrentItem = item;
            return entity;
        }
        public static void UpdateOrAdd<TEntity>(this StateBase<TEntity> entity, TEntity newEntity, Action stateChanged = null) where TEntity : class
        {
            if (IsFromMaui)
            {
                UpdateOrAddAsync(entity, newEntity, stateChanged).Wait();
                return;
            }
            _syncLocalStorageService?.SetItem<TEntity>(entity.TypeName, newEntity);
            entity.CurrentItem = newEntity;
            stateChanged?.Invoke();
            OnChange?.Invoke();
        }
        public static void UpdateOrAdd<TEntity>(this StateBase<TEntity> entity, Action stateChanged = null) where TEntity : class
        {
            if (IsFromMaui)
            {
                UpdateOrAddAsync(entity, stateChanged).Wait();
                return;
            }
            _syncLocalStorageService.SetItem<TEntity>(entity.TypeName, entity.CurrentItem);
            stateChanged?.Invoke();
            OnChange?.Invoke();
        }
        public static async Task<StateBase<TEntity>> GetItemAsync<TEntity>(this StateBase<TEntity> entity) where TEntity : class
        {
            if (entity.CurrentItem != null)
                return entity;
            TEntity item = await _localStorageService.GetItemAsync<TEntity>(entity?.TypeName ?? typeof(TEntity).Name.Replace("State", ""));
            entity.CurrentItem = item;
            return entity;
        }
        public static async Task RemoveAsync<TEntity>(this StateBase<TEntity> entity) where TEntity : class
        {
            await _localStorageService.RemoveItemAsync(entity.TypeName);
            OnChange?.Invoke();
        }
        public static async Task<StateBase<TEntity>> UpdateOrAddAsync<TEntity>(this StateBase<TEntity> entity, TEntity newEntity, Action stateChanged = null) where TEntity : class
        {
            await _localStorageService.SetItemAsync<TEntity>(entity.TypeName, newEntity);
            entity.CurrentItem = newEntity;
            stateChanged?.Invoke();
            OnChange?.Invoke();
            return entity;
        }
        public static async Task UpdateOrAddAsync<TEntity>(this StateBase<TEntity> entity, Action stateChanged = null) where TEntity : class
        {
            await _localStorageService.SetItemAsync<TEntity>(entity.TypeName, entity.CurrentItem);
            stateChanged?.Invoke();
            OnChange?.Invoke();
        }
    }
}