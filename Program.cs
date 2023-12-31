﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBTutorial;
using System.Data.Common;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Connecting to MongoDb
var mongoClient = new MongoClient("mongodb://localhost/");



//var hospitalDb = mongoClient.GetDatabase("hospital");
//var patientCollection = hospitalDb.GetCollection<BsonDocument>("patient");
//var bankDB = mongoClient.GetDatabase("bank");
/*var accountCollection = bankDB.GetCollection<BsonDocument>("account");

var account1 = new BsonDocument
{
	{"account_id","ABC1234" },
	{ "account_holder","XYZZZZ" },
	{ "account_type","saving" },
	{ "balance",12000}
};
*/

//accountCollection.InsertOne(account1);

/*var accountObj = new Account()
{
	AccountID = "MDB829001337",
	AccountHolder = "ZAYYYY",
	AccountType = "Current",
	Balance = 10000
};*/

//var accountCollection1 = bankDB.GetCollection<Account>("account");

//accountCollection1.InsertOne(accountObj);

var accountDb = new AccounMongoDB(mongoClient);

var result = accountDb.GetAccountByName("Ghufran");

Console.WriteLine(result);



bool isContinue = true;
int choice;

do
{
	Console.WriteLine("Select 1 to Add data to db.");
	Console.WriteLine("Select 2 to Get Account by Name.");
	Console.WriteLine("Select 3 to Get all Accounts.");
	Console.WriteLine("Select 4 to update single Account.");
	Console.WriteLine("Select 5 to update manay account");
	Console.WriteLine("Select 6 to Get all dbs.");
	Console.WriteLine("Select 7 to Get all collection in a db.");
	Console.WriteLine("Select 0 to Exit");
	choice = Convert.ToInt32(Console.ReadLine());

	switch (choice)
	{
		case 0: isContinue = false; break;
		case 1: AddData(); break;
		case 2: GetAccountByName(); break;
		case 3: GetAccounts(); break;
		case 4: UpdateSingleRecord(); break;
		case 5: UpdateMultipeRecord(); break;
		case 6: GetAllDbs(); break;
		case 7: GetAllCollections(); break;
	}

} while (isContinue);

void AddData()
{
	Console.WriteLine("Enter Name :");
	var name = Console.ReadLine();
	Console.WriteLine("Enter Accoun Type");
	var accountType = Console.ReadLine();
	Console.WriteLine("Enter Balance");
	var balance = Convert.ToInt32(Console.ReadLine());

	var newAccount = new Account()
	{
		AccountType = accountType,
		AccountHolder = name,
		Balance = balance,
		AccountID = String.Format("100{0}", name.Substring(name.Length - 3))

	};
	accountDb.AddAccount(newAccount);
}

void GetAccountByName()
{
	Console.WriteLine("Enter Account holder Name:");
	var name = Console.ReadLine();
	var record = accountDb.GetAccountByName(name);
	if(record!= null)
	{
		Console.WriteLine("Name {0}\tBalance : {1}\tAccountType : {2}", record.AccountHolder, record.Balance, record.AccountType);
	}
	
}

void GetAccounts()
{
	var listResult = accountDb.GetAccounts();

	foreach (var item in listResult)
	{
		Console.WriteLine(String.Format("Name : {0}\tBalance : {1}\tAccounType : {2}", item.AccountHolder, item.Balance, item.AccountType));
	}
}

void UpdateSingleRecord()
{
	var accountObj = new Account();
	Console.WriteLine("Enter the name :\t");
	var name = Console.ReadLine();
	accountObj.AccountHolder = name;
	Console.WriteLine("You can modify these fiedls, [Balance,AccountType]\nPlease Enter");
	var field1 = Console.ReadLine();
	Console.WriteLine("Enter value for {0}", field1);
	var field1Val = Console.ReadLine();

	if(field1 == "Balance")
	{
		accountObj.Balance = Convert.ToInt32(field1Val);
	}
	else if(field1 == "AccountType")
	{
		accountObj.AccountType = field1Val;
	}
	else
	{
		Console.WriteLine("Invalid Property Named {0} exist in Account object.",field1Val);
	}

	accountDb.UpdateOneAccount(accountObj);
	
	
	
}

void UpdateMultipeRecord()
{

}

void GetAllDbs()
{
	var dbList = mongoClient.ListDatabases().ToList();
	Console.WriteLine("Availiable dbs in database:\t{0}",string.Join(", ",dbList));
	
}


void GetAllCollections()
{
	Console.WriteLine("Enter the db name:\t");
	var dbName = Console.ReadLine();
	var _db = mongoClient.GetDatabase(dbName);
	var collection = _db.ListCollectionNames();
	Console.WriteLine("Collection in Db {0} :\t{1} ",dbName,string.Join(", ",collection.ToList()));
}