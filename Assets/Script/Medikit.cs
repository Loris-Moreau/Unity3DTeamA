using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medikit : MonoBehaviour
{
    public static Medikit instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Medikit dans la sc�ne");
            return;
        }

        instance = this;
    }
}
//T thg,fjhgyurtgedtyujyygdt(szyr u8854656666ujtyughhhhhhghfyrfrtrtghgcdfgcvc  dsqeqfghm��_-�--(--'('(fdh))  == Oeuvre de Gaby le giga Tchad uwu
