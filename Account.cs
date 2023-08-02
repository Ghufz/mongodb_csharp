using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTutorial
{
	internal class Account
	{
		[BsonId()]
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("account_id")]
		public string AccountID { get; set; }

		[BsonElement("account_holder")]
		public string AccountHolder { get; set; }

		[BsonElement("account_type")]
		public string AccountType { get; set; }

		[BsonElement("balance")]
		public double Balance { get; set; }
	}
}
