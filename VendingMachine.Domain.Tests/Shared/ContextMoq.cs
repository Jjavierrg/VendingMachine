namespace VendingMachine.Domain.Tests.Shared
{
    using System.Linq;
    using VendingMachine.Entities;

    internal class ContextMoq
    {
        public ContextMoq()
        {
            var _products = Enumerable.Range(1, 10).Select(i => new Product { Id = i, Name = $"Producto {i}" } );
            var _slots = Enumerable.Range(1, 10).Select(i => new Slot { Id = i, Price = i * 100, Quantity = i * 10, Product = _products.First(x => x.Id == i) });
            var _coins = new Coin[] { 
                new Coin { Id = 1, Description = "10 cent", Value = 10 },
                new Coin { Id = 2, Description = "20 cent", Value = 20 },
                new Coin { Id = 3, Description = "50 cent", Value = 50 },
                new Coin { Id = 4, Description = "100 cent", Value = 100 }
            };

            var _machineCoins = new MachineWalletCoin[] {
                new MachineWalletCoin { Id = 1, CoinId = 1, Coin = _coins.First(x => x.Id == 1), NumberOfCoins = 100},
                new MachineWalletCoin { Id = 2, CoinId = 2, Coin = _coins.First(x => x.Id == 2), NumberOfCoins = 100},
                new MachineWalletCoin { Id = 3, CoinId = 3, Coin = _coins.First(x => x.Id == 3), NumberOfCoins = 100},
                new MachineWalletCoin { Id = 4, CoinId = 4, Coin = _coins.First(x => x.Id == 4), NumberOfCoins = 100},
            };

            ProductRepository = new RepositoryMoq<Product>(_products);
            SlotRepository = new RepositoryMoq<Slot>(_slots);
            CoinRepository = new RepositoryMoq<Coin>(_coins);
            CustomerWalletCoinRepository = new RepositoryMoq<CustomerWalletCoin>();
            MachineWalletCoinRepository = new RepositoryMoq<MachineWalletCoin>(_machineCoins);
        }

        public RepositoryMoq<Product> ProductRepository { get; }
        public RepositoryMoq<Slot> SlotRepository { get; }
        public RepositoryMoq<Coin> CoinRepository { get; }
        public RepositoryMoq<CustomerWalletCoin> CustomerWalletCoinRepository { get; }
        public RepositoryMoq<MachineWalletCoin> MachineWalletCoinRepository { get; }
    }
}
