using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbility : MonoBehaviour
{
    private string _Enemy1 = "Mooping";
    private string _Enemy2 = "Mooping Nomsod";
    private string _Enemy3 = "Taiyaki";
    private string _Enemy4 = "Choclate Taiyaki";
    private string _Enemy5 = "Bluberry Cake";
    private string _Enemy6 = "Red Velvet Cake";
    private string _Enemy7 = "Ant’s Eggs";
    private string _Enemy8 = "Golden Ant’s Eggs";
    private string _Enemy9 = "Croissant";
    private string _Enemy10 = "Lava Croissant";
    private string _Enemy11 = "Kai Jiao";
    private string _Enemy12 = "Kai Jiao Pu";
    public void Ability1(string name)
    {
        if (name == _Enemy1)
        {
            //deal 3 damage and attack down 2 turn
        }
        else if (name == _Enemy2)
        {

        }
        else if (name == _Enemy3)
        {
            //deal 5 damage 2 time
        }
        else if (name == _Enemy4)
        {
            //give player burn 3 turn
        }
        else if (name == _Enemy5)
        {

        }
        else if (name == _Enemy6)
        {

        }
        else if (name == _Enemy7)
        {

        }
        else if (name == _Enemy8)
        {

        }
        else if (name == _Enemy9)
        {

        }
        else if (name == _Enemy10)
        {

        }
        else if (name == _Enemy11)
        {

        }
        else if (name == _Enemy12)
        {

        }
        else
        {
            Debug.Log("Wrong Enemy Name");
        }
    }

    public void Ability2(string name)
    {
        if (name == _Enemy1)
        {

        }
        else if (name == _Enemy2)
        {

        }
        else if (name == _Enemy3)
        {

        }
        else if (name == _Enemy4)
        {

        }
        else if (name == _Enemy5)
        {

        }
        else if (name == _Enemy6)
        {

        }
        else if (name == _Enemy7)
        {

        }
        else if (name == _Enemy8)
        {

        }
        else if (name == _Enemy9)
        {

        }
        else if (name == _Enemy10)
        {

        }
        else if (name == _Enemy11)
        {

        }
        else if (name == _Enemy12)
        {

        }
        else
        {
            Debug.Log("Wrong Enemy Name");
        }
    }

    public void Ability3(string name)
    {
        if (name == _Enemy11)
        {

        }
        else if (name == _Enemy12)
        {

        }
        else
        {
            Ability1(name);
        }
    }


}
