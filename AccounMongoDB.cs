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

		public int UpdateAccount()
		{
			return 0;
		}


	}
}
