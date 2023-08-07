using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTutorial
{
	internal class AccounMongoDB
	{
		private IMongoDatabase _bankDatabase;
		private IMongoCollection<Account> _accounts;
		public AccounMongoDB(MongoClient mongoClient)
		{
			_bankDatabase = mongoClient.GetDatabase("bank");
			_accounts = _bankDatabase.GetCollection<Account>("account");
		}
		public List<Account> Accounts { get; set; }

		public void AddAccount(Account account)
		{
			_accounts.InsertOne(account);
		}

		public void AddMultipleAccount(List<Account> accountList)
		{
			_accounts.InsertMany(accountList);
		}


		public Account GetAccountByName(string Name)
		{
			return _accounts.Find(a => a.AccountHolder.ToLower() == Name.ToLower()).FirstOrDefault();
		}

		public List<Account> GetAccounts()
		{
			Accounts = _accounts.Find(_ => true).ToList();
			return Accounts;
		}

		public long UpdateOneAccount(Account account)
		{
			UpdateDefinition<Account> update=null;
			var filter = Builders<Account>.Filter.Eq(a => a.AccountHolder, account.AccountHolder);
			if (account.Balance != 0)
			{
				update = Builders<Account>.Update.Set(a => a.Balance, account.Balance);
			}
			else if (account.AccountType != null)
			{
				update = Builders<Account>.Update.Set(a => a.AccountType, account.AccountType);
			}
			else
			{
				return 0;
			}
			
			var result = _accounts.UpdateOne(filter,update);
			return result.ModifiedCount;
		}

		public long UpdateManayAccount()
		{
			var filter = Builders<Account>.Filter.Lt(a => a.Balance , 3000);
			var update = Builders<Account>.Update.Inc(a => a.Balance, 500);
			var result = _accounts.UpdateMany(filter, update);
			return result.MatchedCount;
		}
	}
}
