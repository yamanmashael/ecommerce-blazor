
    namespace Blazor.Services
    {
        public class EventService
        {

        public event Action OnSerch;
        public void RaisSerch() => OnSerch?.Invoke();




        public event Action OnCartUpdated;
            public event Action OnCartCleared;
            public event Action OnProductAddedToCart;
            public event Action OnProductRemovedFromCart;
            public event Action OnCartMigration;
            public event Action OnOrderCreated;
            public event Action OnCancelOrder;
           public event Action OnUserLoggedIn;



        public void RaiseCartUpdated() => OnCartUpdated?.Invoke();
            public void RaiseCartCleared() => OnCartCleared?.Invoke();
            public void RaiseProductAddedToCart() => OnProductAddedToCart?.Invoke();
            public void RaiseProductRemovedFromCart() => OnProductRemovedFromCart?.Invoke();
            public void RaiseCartMigration() => OnCartMigration?.Invoke();
            public void RaiseOrderCreated() => OnOrderCreated?.Invoke();
            public void RaiseCancelOrder() => OnCancelOrder?.Invoke();
            public void RaiseUserLoggedIn() => OnUserLoggedIn?.Invoke();



        }
    }


