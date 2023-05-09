﻿namespace MoogleEngine;
    
public static class Moogle
{
    static DataFolder Content;
    public static bool Initied = false;
    public static void Init()
    {
        if(!Initied)
        {
            Initied = true;

            Content = new DataFolder("../Content");
        }
    }
    public static SearchResult Query(string query) 
    {   
        Init();
        
        if(!string.IsNullOrEmpty(query))
        {   
            SearchResult result = Engine.Query(query,Content);
            return result;
        }

        return new SearchResult();
    }
}
