using System;
using System.IO; // EnumerateFiles, StreamReader
using System.Collections.Generic; // Dictionary
using System.Text.RegularExpressions; // Regex 
using System.Text; //NormalizationForm

namespace MoogleEngine;

public class Tools
{
    private static string RemoveAccentsAndPuntuations(string inputString) // método que elimina todos los signos de puntuacion
    {
        return Regex.Replace(inputString.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", " "); 
    }

    public static string[] TxtProcesser(string inputString) // método que procesa el contenido de un archivo y devuelve un array de palabras normalizadas
    {
        inputString = inputString.ToLower(); // llevamos todo el contenido a minusculas
        inputString = RemoveAccentsAndPuntuations(inputString); // removemos todos los signos de puntuacion
        string[] split = inputString.Split(' '); // dividimos en palabras y guardamos en un array

        List<string> almostWords = new List<string>(); // creamos una lista
        
        foreach(string word in split) // si la palabra no es nula la guardamos en la lista
        {
            if(word != "")
            almostWords.Add(word);
        }

        string[] words = almostWords.ToArray(); // llevamos la lista a un array que devolvemos

        return words;
    }

    public static string Transform(string value)//metodo que elimina los caracteres incomodos al leer un txt
    {
        char[] guide = { '\r', '\n', '(', ')', '*', '{', '}', '´', '`' ,',','.',':'};
        foreach (char a in guide)
        {
            value = value.Replace(a, ' ');
        }
        return value;
    }
}