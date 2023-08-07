# mongodb_csharp
Sample code to go through mongodb csharp driver

## Working with MongoDB Documents in C#

Review the following code, which demonstrates how to represent a document in C#.

### BsonDocument

Use MongoDB.Bson to represent a document with BsonDocument. Here's an example:
```
using MongoDB.Bson;

var document = new BsonDocument
{
   { "account_id", "MDB829001337" },
   { "account_holder", "Linus Torvalds" },
   { "account_type", "checking" },
   { "balance", 50352434 }
};
```

### C# Class (POCOs)

Each public property maps to a field in the BSON document.

    The BsonId attribute specifies a field that must always be unique.
    The BsonRepresentation attribute maps a C# type to a specific BSON type.
    The BsonElement attribute maps to the BSON field name.

Here's an example:
```
internal class Account
{
   [BsonId]
   [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]     
   public string Id { get; set; }
     
   [BsonElement("account_id")]
   public string AccountId { get; set; }

   [BsonElement("account_holder")]
   public string AccountHolder { get; set; }

   [BsonElement("account_type")]
   public string AccountType { get; set; } 

   [BsonRepresentation(BsonType.Decimal128)]
   [BsonElement("balance")]
   public decimal Balance { get; set; }

   [BsonElement("transfers_complete")]
   public string[] TransfersCompleted { get; set; }    
}
```
## Inserting a Document in C# Applications

Review the following code, which demonstrates how to insert a single document and multiple documents into a collection.
Insert a Single Document

Use the GetCollection() method to access the MongoCollection object, which is used to represent the specified collection. For example:
```
var accountsCollection = database.GetCollection<Account>("account");
```

#### C# Class

Use the InsertOne() method on the accountsCollection object to insert a document into the collection. Within the parentheses of InsertOne(), include an object that contains the document data. Here's an example:
```
var accountsCollection = database.GetCollection<Account>("account");

var newAccount = new Account
{
    AccountId = "MDB829001337",
    AccountHolder = "Linus Torvalds",
    AccountType = "checking",
    Balance = 50352434
};

accountsCollection.InsertOne(newAccount);

```

#### BsonDocument

Use the BsonDocument class to insert a document into the collection. Within the parentheses of InsertOne(), include an object that contains the document data. For example:
```
var accountsCollection = database.GetCollection<BsonDocument>("account");

var document = new BsonDocument
{
   { "account_id", "MDB829001337" },
   { "account_holder", "Linus Torvalds" },
   { "account_type", "checking" },
   { "balance", 50352434 }
};

accountsCollection.InsertOne(document);
```

#### Async

Use the InsertOneAsync() method on the accountsCollection object to insert a document asynchronously into the collection. Within the parentheses of InsertOne(), include an object that contains the document data. For example:
```
public async Task AddAccount()
{
   var newAccount = new Account
   {
       AccountId = "MDB829001337",
       AccountHolder = "Linus Torvalds",
       AccountType = "checking",
       Balance = 50352434
   };

   await accountsCollection.InsertOneAsync(document);
}

```

### Insert Many Documents

This section shows three ways to insert multiple documents into a collection.

#### C# Class

Use the InsertMany() method on the accountsCollection object to insert multiple documents into the collection.

var accountsCollection = database.GetCollection<Account>("accounts");
```
var accountA = new Account
{
    AccountId = "MDB829001337",
    AccountHolder = "Linus Torvalds",
    AccountType = "checking",
    Balance = 50352434
};

var accountB = new Account
{
    AccountId = "MDB011235813",
    AccountHolder = "Ada Lovelace",
    AccountType = "checking",
    Balance = 60218
};

accountsCollection.InsertMany(new List<Account>() { accountA, accountB });
```

#### BsonDocument

Use the BsonDocument class to insert a document into the collection. Within the parentheses of InsertMany(), include an object that contains the document data. For example:
```
var accountsCollection = database.GetCollection<BsonDocument>("account");

var documents = new[]
{
    new BsonDocument
            {
                { "account_id", "MDB011235813" },
                { "account_holder", "Ada Lovelace" },
                { "account_type", "checking" },
                { "balance", 60218 }
            },
    new BsonDocument
            {
                { "account_id", "MDB829000001" },
                { "account_holder", "Muhammad ibn Musa al-Khwarizmi" },
                { "account_type", "savings" },
                { "balance", 267914296 }
            }
};

accountsCollection.InsertMany(documents);
```

#### Async

Use the InsertManyAsync() method on the accountsCollection object to insert multiple documents asynchronously into the collection. Here's an example:
```
public async Task AddAccounts()
{
   var accountA = new Account
{
    AccountId = "MDB829001337",
    AccountHolder = "Linus Torvalds",
    AccountType = "checking",
    Balance = 50352434
};

var accountB = new Account
{
    AccountId = "MDB011235813",
    AccountHolder = "Ada Lovelace",
    AccountType = "checking",
    Balance = 60218
};

   await accountsCollection.InsertManyAsync(new List<Account>() { accountA, accountB });

}
```

## Querying a MongoDB Collection in C# Applications

Review the following code, which demonstrates how to query documents in MongoDB with C#.


### Find a Document with FirstOrDefault

In the following example, the Find() command with a LINQ expression matches the AccountID field. The FirstOrDefault() method returns the first or default result.
```
var account = accountsCollection
   .Find(a => a.AccountId == "MDB829001337")
   .FirstOrDefault();
```

### Find a Document with FindAsync and FirstOrDefault

The FindAsync() command with a LINQ expression matches the AccountID field. The FirstOrDefault() method returns the first or default result. For example:
```
var accounts = await accountsCollection
   .FindAsync(a => a.AccountId == "MDB829001337");

var account = accounts.FirstOrDefault();
```

### Find a Document with ToList

The Find() command with a LINQ expression matches all documents in the collection. The ToList() method returns a list of results. For example:
```
var accounts = accountsCollection.Find(_ => true).ToList();
```

### Find a Document with Multiple LINQ Methods

The Find() command with a LINQ expression filters documents by AccountType (in this case, “checking”), sorts the results in descending order by the Balance, skips the first 5 results, and returns only 20 documents due to the limit.
```
accountsCollection
   .Find(a => a.AccountType == "checking")
   .SortByDescending(a => a.Balance)
   .Skip(5)
   .Limit(20);

```

### Find a Document with the Builders Class

Use the Builders class to match all documents in the collection with an _id field equal to the specified value. For example:
```
var filter = Builders<BsonDocument>
   .Filter
   .Eq("_id", new    
      ObjectId("62d6e04ecab6d8e1304974ae"));

var document = accountsCollection
   .Find(filter)
   .FirstOrDefault();
```
Use the Builders class to match all documents in the collection with a balance field greater than 1000. For example:
```
var filter = Builders<BsonDocument>
   .Filter
   .Gt("balance", 1000);

var documents = await accountsCollection
   .FindAsync(filter);
```
## Updating Documents in C# Applications

Review the following code, which demonstrates how to update documents in MongoDB with C#.


### Update a Single Document

The following example demonstrates how to update a single document. First, create a filter definition with the .Filter method on the Builders class, which returns the account with an AccountId equal to �MDB951086017�. Next, create an update definition that will set the balance to 5000. Finally, use the UpdateOne() method to update the document.
```code
var filter = Builders<Account>
   .Filter
   .Eq(a => a.AccountId, "MDB951086017");

var update = Builders<Account>
   .Update
   .Set(a=>a.Balance, 5000);

var result = accountCollection.UpdateOne(filter, update);

Console.WriteLine(result.ModifiedCount);
```

### Update a Single Document Asynchronously

The UpdateOneAsync() command updates a single document in the collection asynchronously. For example:
```
var result = await accountsCollection.UpdateOneAsync(filter, update);

Console.WriteLine(result.ModifiedCount);
```

### Update Multiple Documents

Use the UpdateMany() method to update multiple documents in a single operation. Just like the UpdateOne() method, the UpdateMany() method accepts a query and an update. Here's an example:
```
var filter = Builders<Account>
   .Filter
   .Eq(a => a.AccountType, "checking");

var update = Builders<Account>
   .Update
   .Inc(a => a.Balance, 5);

var updateResult = accountCollection
   .UpdateMany(filter, update);

Console.WriteLine(updateResult.ModifiedCount);
```

### Update Multiple Documents Asynchronously

The UpdateManyAsync() command updates multiple documents in the collection asynchronously. For example:
```
var filter = Builders<BsonDocument>
   .Filter
   .Lt("balance", 500);

var update = Builders<BsonDocument>
   .Update
   .Inc("balance", 10);

var result = await accountsCollection
   .UpdateManyAsync(filter, update);

Console.WriteLine(result.ModifiedCount);
```

## Deleting Documents in C# Applications

Review the following code, which demonstrates how to delete documents in MongoDB with C#.

### Delete a Single Document

To delete a single document, use the DeleteOne() method, which accepts a query filter that matches the document that you want to delete. DeletedCount tells you how many documents were found by the filter and were deleted. Here's an example:
```
var accountsCollection = 
  database.GetCollection<Account>("Account");

var result  = accountsCollection
   .DeleteOne(a => a.AccountId == "MDB333829449");

Console.WriteLine(result.DeletedCount);
```

### Delete a Single Document Asynchronously

To delete a single document asynchronously, use the DeleteOneAsync() method, which accepts a query filter that matches the document that you want to delete. We use a Builders class that matches a document based on the specified _id. Async methods can be used with builders or LINQ. Here's an example:
```
var filter = Builders<BsonDocument>
   .Filter
   .Eq("_id", new    
      ObjectId("63050518546c1e9d2d16ce4d"));

var accounts = await accountsCollection
   .DeleteOneAsync(filter);
```

### Delete Multiple Documents

To delete multiple documents, use the DeleteMany() method, which accepts a query filter that matches the documents that you want to delete. Once the documents are successfully deleted, the method returns an instance of DeleteResult, which enables the retrieval of information such as the number of documents that were deleted. For example:

```
var deleteResult = accountCollection
   .DeleteMany(a => a.Balance < 500);

Console.WriteLine(result.DeleteCount)
```

### Delete Multiple Documents Asynchronously

To delete multiple documents asynchronously, use the DeleteMany() method, which accepts a query filter that matches the documents that you want to delete. Once the documents are successfully deleted, the method returns an instance of DeleteResult, which enables the retrieval of information such as the number of documents that were deleted. We use a Builders class that matches a document based on the specified account_type. Async methods can be used with builders or LINQ. For example:
```
var filter = Builders<BsonDocument>
   .Filter
   .Eq("account_type", "checking");

var deleteResult = await accountsCollection
   .DeleteManyAsync(filter);

Console.WriteLine(deleteResult.DeletedCount);
```

## Creating MongoDB Transactions in C# Applications

Review the following code, which demonstrates how to create a multi-document transaction in MongoDB with C#.
Multi-Document Transaction

The following are the steps and the code to create a multi-document transaction in MongoDB with C#. The transaction is started by using the session’s WithTransaction() method. Then, we define the sequence of operations to perform inside the transaction. Here are the steps:

   - Start a new session.
   - Begin a transaction with the WithTransaction() method on the session.
   - Create variables that will be used in the transaction.
   - Obtain the user accounts information that will be used in the transaction.
   - Create the transfer document.
   - Update the user accounts.
   - Insert the transfer document.
   - Commit the transaction.

Here's the code:
```
using (var session = client.StartSession())
{

    // Define the sequence of operations to perform inside the transactions
    session.WithTransaction(
        (s, ct) =>
        {
            var fromId = "MDB310054629";
            var toId = "MDB546986470";

            // Create the transfer_id and amount being transfered
            var transferId = "TR02081994";
            var transferAmount = 20;

            // Obtain the account that the money will be coming from
            var fromAccountResult = accountsCollection.Find(e => e.AccountId == fromId).FirstOrDefault();
            // Get the balance and id of the account that the money will be coming from
            var fromAccountBalance = fromAccountResult.Balance - transferAmount;
            var fromAccountId = fromAccountResult.AccountId;

            Console.WriteLine(fromAccountBalance.GetType());

            // Obtain the account that the money will be going to
            var toAccountResult = accountsCollection.Find(e => e.AccountId == toId).FirstOrDefault();
            // Get the balance and id of the account that the money will be going to
            var toAccountBalance = toAccountResult.Balance + transferAmount;
            var toAccountId = toAccountResult.AccountId;

            // Create the transfer record
            var transferDocument = new Transfers
            {
                TransferId = transferId,
                ToAccount = toAccountId,
                FromAccount = fromAccountId,
                Amount = transferAmount
            };

            // Update the balance and transfer array for each account
            var fromAccountUpdateBalance = Builders<Accounts>.Update.Set("balance", fromAccountBalance);
            var fromAccountFilter = Builders<Accounts>.Filter.Eq("account_id", fromId);
            accountsCollection.UpdateOne(fromAccountFilter, fromAccountUpdateBalance);

            var fromAccountUpdateTransfers = Builders<Accounts>.Update.Push("transfers_complete", transferId);
            accountsCollection.UpdateOne(fromAccountFilter, fromAccountUpdateTransfers);

            var toAccountUpdateBalance = Builders<Accounts>.Update.Set("balance", toAccountBalance);
            var toAccountFilter = Builders<Accounts>.Filter.Eq("account_id", toId);
            accountsCollection.UpdateOne(toAccountFilter, toAccountUpdateBalance);
            var toAccountUpdateTransfers = Builders<Accounts>.Update.Push("transfers_complete", transferId);

            // Insert transfer doc
            transfersCollection.InsertOne(transferDocument);
            Console.WriteLine("Transaction complete!");
            return "Inserted into collections in different databases";
        });
}
```
