using UnityEngine;
using UnityEngine.UI;
using System;


public static class ButtonExtension
{
    public static void AddEvenListener<T>(this Button button, T parma, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            
           
                OnClick(parma);
            
        });


} }
