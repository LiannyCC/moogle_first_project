using System;
using System.Collections.Generic; // Dictionary

namespace MoogleEngine;

public static class Engine
{   
    #region Area de metodos de la clase

        public static SearchResult Query(string query, DataFolder Content) // metodo que recibe una query y un contenedor de archivos y devuelve el resultado de la busqueda
        {   
            string suggestion = query; // asiganamos a la sugerencia el mismo string que la query
            Query ToSearch = new Query(query); // creamos con la query un objeto de tipo Query

            Dictionary < string, Dictionary<string, float>> Docs = Content.Relevance;

            float[] docs_score = new float[Content.NumberOfFiles]; // creamos el array en el que guardaremos los scores de los documentos

            int count = 0;
            foreach (string file in Docs.Keys) // por cada documento
            {   
                docs_score[count]= ScoreCalculator(ToSearch, Docs, file); // calculamos y guardamos su score
                count++; 
            }

            List <SearchItem> items = new List<SearchItem>(); // creamos la lista de SearchItems

            if(docs_score != null)
            {
                for(int i = 0; i < docs_score.Length; i++) // por cada valor del array de los scores relacionado aun documento
                {   
                    if(docs_score[i] > 0)
                    {   
                        string snippet = Content.Files[i].FragmentWithWords(ToSearch.QueryWordsArray, Content.Files[i].FileRoot, DataFolder.IDF); // obtenemos el snippet
                        string title = Content.Files[i].FileName; // obtenemos el titulo del documento
                        float score = docs_score[i]; // obtenemos el score
                        SearchItem item = new SearchItem(title, snippet, score); // creamos el objeto de tipo SearchItem
                        items.Add(item); // lo anadimos a la lista de SearchItems 
                    }
                }
            }

            SearchItem[] Items = items.ToArray(); // llevamos la lista a un array
            Items = Sort(Items); // ordenamos sus valor

            System.Console.WriteLine("Los resultados han sido devueltos");

            foreach(SearchItem item in Items)
            {
                System.Console.WriteLine($"el item {item.Title} tiene un score de {item.Score}");
            }

            return new SearchResult(Items, suggestion);
        }

        private static float ScoreCalculator (Query ToSearch, Dictionary < string, Dictionary<string, float>> Docs, string file) // método que calcula el score
        {
            float dotProduct = 0;
            float dim1 = 0;
            float dim2 = 0;

            foreach(string word in Docs[file].Keys)
                {
                    if(!ToSearch.DataQuery.ContainsKey(word))
                        dotProduct += 0;
                    else
                    {
                        dotProduct += ToSearch.DataQuery[word] * Docs[file][word];  
                        dim2 += (float)Math.Pow(ToSearch.DataQuery[word], 2); //para calcular la norma del vector 2
                    }

                    dim1 += (float)Math.Pow(Docs[file][word], 2); // para calcular la norma del vector 1
                }
            return dotProduct / ((dim1==0 || dim2==0)?1:(float)(Math.Sqrt(dim1) * Math.Sqrt(dim2))); // distancia coseno 
        }

        private static SearchItem[] Sort(SearchItem[] docs_score)// metodo que ordena el array de SearchItem segun sus scores
        {
            for (int i = 0; i < docs_score.Length; i++)
            {
                for (int j = i; j < docs_score.Length; j++)
                {    if (docs_score[j].Score > docs_score[i].Score)
                    {
                        SearchItem temp = docs_score[j];
                        docs_score[j] = docs_score[i];
                        docs_score[i] = temp;
                    }
                }
            }

            return docs_score;
        }    
           #endregion
}