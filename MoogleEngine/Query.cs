using System;
using System.IO;
using System.Collections.Generic; 
using System.Text.RegularExpressions; 
using System.Text;

namespace MoogleEngine;

    public class Query
    {
        #region Área de declaración de propiedades de la clase

        public string InputQuery {get; set;} // string introducido por el usuario
        public Dictionary <string, float> DataQuery; // diccionario que almacena la relevancia de cada palabra de la query

        public string[] QueryWordsArray {get; private set;} // array que las palabras de la query

        public int QueryWords {get; private set;} // cantidad de palabras de la query

        #endregion

        #region Área del constructor de la clase

        public Query(string input)
        {
            InputQuery = input;

            QueryWordsArray = Tools.TxtProcesser(InputQuery); // procesamos la query

            QueryWords = QueryWordsArray.Length; 

            DataQuery = new Dictionary <string, float>(); 
        
            foreach(string word in QueryWordsArray) // por cada palabra en la query
            {
                if(!DataQuery.Keys.Contains(word)) // si el diccionario no contenia la palabra
                DataQuery.Add(word, 1); // la anadimos y le asignamos una frecuencia de 1
 
                else // si la palabra ya se encontraba en el diccionario
                DataQuery[word]++; //aumentamos su frecuencia en 1
            }

            foreach(string key in DataQuery.Keys)
            {
                DataQuery[key] = DataQuery[key]/QueryWords; // calculamos el TF de la query
                if(DataFolder.IDF.ContainsKey(key))
                {
                    DataQuery[key] = DataQuery[key] * DataFolder.IDF[key];
                } // multiplicamos el TF*IDF para obtener la relevancia de cada palabra de la query
            }

            System.Console.WriteLine($"La Query {input} ha sido cargada");
        }
        #endregion
    }

    