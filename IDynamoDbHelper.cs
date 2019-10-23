using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDbWriter
{
    public interface IDynamoDbHelper
    {
        /// <summary>
        /// Get all the records from the given table
        /// </summary>
        /// <typeparam name="T">Table object</typeparam>
        /// <returns></returns>
        Task<IList<T>> GetAll<T>();

        /// <summary>
        /// Creates new table in DynamoDB
        /// ScalarAttributeType.S = string
        /// ScalarAttributeType.B = binary
        /// ScalarAttributeType.N = integer
        /// </summary>
        /// <param name="tableName">name of the table to create</param>
        /// <param name="hashKey">Hash key name</param>
        /// <param name="haskKeyType">Hask key type</param>
        /// <param name="rangeKey">range key name</param>
        /// <param name="rangeKeyType">range key type</param>
        Task CreateTable(string tableName, string hashKey, ScalarAttributeType hashKeyType, string rangeKey = null, ScalarAttributeType rangeKeyType = null);

        /// <summary>
        /// Get the rows from the given table which matches the given key and conditions
        /// </summary>
        /// <typeparam name="T">Table object</typeparam>
        /// <param name="keyValue">hash key value</param>
        /// <param name="scanConditions">any other scan conditions</param>
        /// <returns></returns>
        Task<IList<T>> GetRows<T>(object keyValue, List<ScanCondition> scanConditions = null);

        /// <summary>
        /// Get the rows from the given table which matches the given conditions
        /// </summary>
        /// <typeparam name="T"> Table object</typeparam>
        /// <param name="scanConditions"></param>
        /// <returns></returns>
        Task<IList<T>> GetRows<T>(List<ScanCondition> scanConditions);

        /// <summary>
        /// Gets a record which matches the given key value
        /// </summary>
        /// <typeparam name="T">Table object</typeparam>
        /// <param name="keyValue">Hash key value</param>
        /// <returns></returns>
        Task<T> Load<T>(object keyValue);

        /// <summary>
        /// Saves the given record in the table
        /// </summary>
        /// <typeparam name="T">Table object</typeparam>
        /// <param name="document">Record to save in the table</param>
        /// <returns></returns>
        Task Save<T>(T document);

        /// <summary>
        /// Deletes the given record in the table
        /// </summary>
        /// <typeparam name="T">Table object</typeparam>
        /// <param name="document">Record to be removed from the table</param>
        /// <returns></returns>
        Task Delete<T>(T document);

        /// <summary>
        /// Saves batch of records in the table
        /// </summary>
        /// <typeparam name="T">Table object</typeparam>
        /// <param name="documents">Records to be saved</param>
        /// <returns></returns>
        Task BatchSave<T>(IEnumerable<T> documents);

        /// <summary>
        /// Deletes batch of records in the table
        /// </summary>
        /// <typeparam name="T">Table object</typeparam>
        /// <param name="documents">Records to be delete</param>
        /// <returns></returns>
        Task BatchDelete<T>(IEnumerable<T> documents);
    }
}