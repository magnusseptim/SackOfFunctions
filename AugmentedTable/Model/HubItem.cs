using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AugmentedTable.Model
{
    public class HubItem : TableEntity
    {
        public HubItem()
        {
            PartitionKey = Guid.NewGuid().ToString();
            RowKey = Guid.NewGuid().ToString();
        }
        public string BlobUri { get; set; }
    }
}
