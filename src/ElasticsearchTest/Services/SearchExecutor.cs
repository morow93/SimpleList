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
            var resp = _client.CreateIndex(_indexName, IndexSettingsSelector());
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

            //var resp =
            //    _client.Search<Card>(
            //        sd =>
            //            sd.Query(q => q.QueryString(s => s.Fields(qf => qf.Field(sf => sf.Title)).Query(query).MinimumShouldMatch(MinimumShouldMatch.Percentage(100)))
            //        )
            //    );

            //var resp = _client.Search<Card>(
            //    body =>
            //        body
            //            .Index(_indexName)
            //                .Query(
            //                    q => q.Wildcard(qd => qd.Field(f => f.Title).Value(query.ToLower() + "*"))));

            return resp.Documents.ToList();

        }

        private static Func<CreateIndexDescriptor, CreateIndexDescriptor> IndexSettingsSelector()
        {
            return ci => ci.Settings(ss => ss.Analysis(
                a => a.Analyzers(fd => fd
                        .Custom("index_raw", ca => ca
                            .Tokenizer("standard")
                            .Filters(new List<string>
                            {
                                "lowercase"
                            }))                    
                        )
                    )
                );
        }

        public Boolean Remove(Int32 id)
        {
            var resp = _client.Delete<Card>(id);
            return resp.IsValid;
        }
    }
}
