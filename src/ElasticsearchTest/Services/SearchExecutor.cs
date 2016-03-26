using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticsearchTest.Services
{
    public class SearchExecutor
    {
        private String _indexName = "azaza";
        private ElasticClient _client
        {
            get
            {
                var node = new Uri("http://localhost:9200");

                var settings = new ConnectionSettings(
                    node
                );
                settings.DefaultIndex(_indexName);
                return new ElasticClient(settings);
            }
        }

        public SearchExecutor()
        {
            var respDelete = _client.DeleteIndex(_indexName);
            var respCreate = _client.CreateIndex(_indexName);
        }
        
        public Boolean AddOrUpdateIndex(Card card)
        {         
            var resp = _client.Index(card);
            return resp.IsValid;             
        }
        
        public List<Card> Search(String query)
        {
            var resp =
                _client.Search<Card>(
                    sd =>
                        sd.Query(q => q.SimpleQueryString(s => s.Query(query)
                        )
                    )
                );
            return resp.Documents.ToList();
        }

        public Boolean Remove(Int32 id)
        {
            var resp = _client.Delete<Card>(id);
            return resp.IsValid;
        }
    }
}
