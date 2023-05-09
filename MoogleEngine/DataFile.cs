using System;
using System.IO; // EnumerateFiles, StreamReader
using System.Collections.Generic; // Dictionary
using System.Text.RegularExpressions; // Regex 
using System.Text; //NormalizationForm

namespace MoogleEngine;

public class DataFile
    {   
        #region Área de declaración de propiedades de clase

        public string FileRoot {get; private set;} // ruta del archivo
        public string FileName {get; private set;} // nombre del archivo
        public string FileContent {get; private set;} // contenido del archivo sin procesar
        public int FileWords {get; private set;} // cantidad de palabras por archivo
        public string[] AllWordsOnFile {get; set;} // array que contiene todas las palabras del archivo
        public Dictionary <string, float> WordFreq {get; set;} // diccionario que contiene el conjunto de palabras de un documento y su TF

        #endregion


        #region Área del constructor de la clase

        public DataFile(string root)
        {   
            FileRoot = root;
            FileName = Path.GetFileName(root); // obtenemos el nombre del archivo con su extension
            FileName = Path.ChangeExtension(FileName, null); // anulamos la extension

            StreamReader reader = new StreamReader(root); // leemos el contenido del archivo
            FileContent = reader.ReadToEnd();
            reader.Close(); 

            AllWordsOnFile = Tools.TxtProcesser(FileContent); // procesamos el contenido del archivo

            FileWords = AllWordsOnFile.Length; 

            WordFreq = new Dictionary<string, float>();

            float maxFreq = 0;
            foreach(string word in AllWordsOnFile)
                {  
                    if(!WordFreq.Keys.Contains(word)) // si la palabra no estaba en el documento la cargamos y aparece 1 vez
                    {
                        WordFreq.Add(word, 1);
                    }
                    else // si la palabra ya estaba en el documento aumentamos su valor en 1
                    {
                        WordFreq[word]++;
                        maxFreq = Math.Max(maxFreq , WordFreq[word]);
                    }
                }

                foreach(string key in WordFreq.Keys)
                {
                    WordFreq[key] = WordFreq[key]/maxFreq; // tomamos el TF de un termino en un documento como el numero de apariciones del termino en un documento / numero total de palabras del documento
                }
        }
        #endregion

        #region Metodos de clase

        string[] Partition(string[]words,int stratindex,int endindex)//método que devuelve una partición de un array de palabras
        {
            string[] result = new string[endindex - stratindex];
            int position = 0;
            for(int i = stratindex; i < endindex; i++)
            {
                result[position] = words[i];
                position++;
            }
            return result;
        }
        string[] WordsImportant(string[] AllWordsOnFile, Dictionary <string, float> IDF) // metodo que nos devuelve un array con las palabras que tengan relevancia
        {
            List<string> result = new List<string>();

            foreach(string word in AllWordsOnFile)
            {   
                if(IDF.ContainsKey(word) && IDF[word] != 0 && word.Length > 3)
                result.Add(word);
            }

            return result.ToArray();
        }
        public string FragmentWithWords(string[] query, string root, Dictionary <string, float> IDF) // metodo que devuelve el fragmento de texto de un archivo que contiene la mayor cantidad de palabras de las que nos piden
        {
            StreamReader reader = new StreamReader(root);
            string content = reader.ReadToEnd().ToLower();
            reader.Close();
            content = Tools.Transform(content);

            string[] words = content.Split(' ');

            int part1 = 0; // contadores para comparar
            int part2 = 0;

            string[] Query = WordsImportant(query, IDF); // extraemos las palabras que nos interesan

            while (words.Length > 80) // solo devolveremos un maximo de 80 palabras
            {
                // partimos a la mitad el array
                string[] Part1 = Partition(words, 0, words.Length / 2);
                string[] Part2 = Partition(words, words.Length / 2, words.Length);

                int count = 0;

                for (int i = 0; i < Math.Min(Part1.Length, Part2.Length); i++)
                {
                    count++;
                    if (Query.Contains(Part1[i]))
                    {
                        part1++;
                    }
                    if (Query.Contains(Part2[i]))
                    {
                        part2++;
                    }
                }
                if (Part1.Length > Part2.Length)
                {
                    for (int i = count; i < Part1.Length; i++)
                    {
                        if (Query.Contains(Part1[i]))
                        {
                            part1++;
                        }
                    }
                }
                if (Part2.Length > Part1.Length)
                {
                    for (int i = count; i < Part2.Length; i++)
                    {
                        if (Query.Contains(Part2[i]))
                        {
                            part2++;
                        }
                    }
                }
                if (part1 >= part2) // nos quedamos con la que mas resultados tuvo
                    words = Part1;
                else
                    words = Part2;
                part1 = 0;
                part2 = 0;
            }

            string result = "";

            foreach (string a in words) // generamos el texto a devolver
            {
                if (Query.Contains(a))
                    result += "**" + a + "** ";
                else
                    result += a + " ";
            }

            return "........" + result + "........";
        }

        #endregion
    }