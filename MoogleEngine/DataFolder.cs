using System;
using System.IO; // EnumerateFiles, StreamReader
using System.Collections.Generic; // Dictionary
using System.Text.RegularExpressions; // Regex 
using System.Text; //NormalizationForm

namespace MoogleEngine;

public class DataFolder
    {   
        #region Área de declaración de propiedades de clase

        public string[] FilesRoot {get; set;} // array para guardar las rutas de los archivos
        public DataFile[] Files {get; set;} // array para guardar los archivos
        public int NumberOfFiles {get; private set;} // cantidad de archivos a nivel de carpeta
        public Dictionary <string, Dictionary <string, float>> TF; // Diccionario donde se guardarán los TF de las palabras por documento
        public Dictionary <string, Dictionary <string, float>> Relevance; // Diccionario donde se guardarán los cálculos de TF*IDF de cada término por cada documento
        public static Dictionary <string, float> IDF {get; set;} //Diccionario que contiene los IDF por palabra

    
        #endregion        


        #region Área del constructor de la clase

        public DataFolder(string root)
        {   
            TF = new Dictionary<string, Dictionary<string,float>>(); 
            Relevance = new Dictionary<string, Dictionary<string,float>>();
            IDF = new Dictionary<string, float>();

            FilesRoot = Directory.EnumerateFiles(root, "*.txt").ToArray(); // obtenemos las rutas de todos los archivos de la carpeta
            NumberOfFiles = FilesRoot.Length;

            Files = new DataFile[NumberOfFiles];

            Console.WriteLine("Cargando archivos...");

            int count = 0;

            foreach(string path in FilesRoot) // por cada ruta en el array de rutas
            {
                DataFile file = new DataFile(path); // creamos un objeto de tipo DataFile
                Files[count] = file;
                count++;

                System.Console.WriteLine($"Cargando archivo {file.FileName}");

                TF.Add(file.FileName, file.WordFreq); // agregamos la frecuencia de cada palabra en cada archivo al diccionario de TFs
                Relevance.Add(file.FileName, file.WordFreq); // agregamos la frecuencia de cada palabra en cada archivo al diccionario de Relevancias para modificar despues su valor
            }

            foreach(DataFile file in Files) // objeto de tipo DataFile en el array de DataFiles
            {
                foreach(string word in file.WordFreq.Keys) // por cada palabra del DataFile
                {
                    if(!IDF.ContainsKey(word)) // si la palabra no esta contenida en el diccionario de los IDFs
                    IDF.Add(word, idfCalculator(word)); // la agregamos al diccionario y calculamos su IDF

                    Relevance[file.FileName][word] = relevanceCalculator(file.FileName, word); // calculamos el valor de la relevancia para cada palabra de cada archivo en el diccionario de las relevancias
                }
            }

            System.Console.WriteLine($"Han sido cargados {NumberOfFiles} archivos");

        }
        #endregion

        #region Metodos de clase

        private int countContains(string word) // método que cuenta en cuántos documentos está contenida una palabra
        {
            int count = 0;
            foreach (string doc in TF.Keys)
            {
                if (TF[doc].ContainsKey(word))
                {
                    count++;
                }
            }
            return count;
        }

        private float idfCalculator(string word) // método que calcula el tf de una palabra (log natural de la razón entre el número total de archivos y el número de archivos que contienen dicha palabra)
        {
            float IDF = (float)Math.Log10((float)NumberOfFiles / (float)countContains(word));
            return IDF;
        }
        
        private float relevanceCalculator(string document, string word) // metodo que calcula la relevancia de un documento en relaci'on a una palabra
        {
            float tf = TF[document][word];
            float idf = idfCalculator(word);

            float relevance = tf * idf;

            return relevance;
        }
        
        #endregion
    }
