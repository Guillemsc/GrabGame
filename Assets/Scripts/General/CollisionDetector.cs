using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public delegate void DelOnCollision2D(Collision2D coll);
    public delegate void DelOnTrigger2D(Collider2D coll);
    public delegate void DelOnCollision(Collision coll);
    public delegate void DelOnTrigger(Collider coll);

    // -------

    public void SuscribeOnCollisionEnter(DelOnCollision callback)
    {
        on_collision_enter += callback;
    }

    public void UnSuscribeOnCollisionEnter(DelOnCollision callback)
    {
        on_collision_enter -= callback;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (on_collision_enter != null)
            on_collision_enter(collision);
    }

    // -------

    public void SuscribeOnCollisionExit(DelOnCollision callback)
    {
        on_collision_exit += callback;
    }

    public void UnSuscribeOnCollisionExit(DelOnCollision callback)
    {
        on_collision_exit -= callback;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (on_collision_exit != null)
            on_collision_exit(collision);
    }

    // -------

    public void SuscribeOnCollisionStay(DelOnCollision callback)
    {
        on_collision_stay += callback;
    }

    public void UnSuscribeOnCollisionStay(DelOnCollision callback)
    {
        on_collision_stay -= callback;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (on_collision_stay != null)
            on_collision_stay(collision);
    }

    // -------

    public void SuscribeOnTriggerEnter(DelOnTrigger callback)
    {
        on_trigger_enter += callback;
    }

    public void UnSuscribeOnTriggerEnter(DelOnTrigger callback)
    {
        on_trigger_enter -= callback;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (on_trigger_enter != null)
            on_trigger_enter(collision);
    }

    // -------

    public void SuscribeOnTriggerExit(DelOnTrigger callback)
    {
        on_trigger_exit += callback;
    }

    public void UnSuscribeOnTriggerExit(DelOnTrigger callback)
    {
        on_trigger_exit -= callback;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (on_trigger_exit != null)
            on_trigger_exit(collision);
    }

    // -------

    public void SuscribeOnTriggerStay(DelOnTrigger callback)
    {
        on_trigger_stay += callback;
    }

    public void UnSuscribeOnTriggerStay(DelOnTrigger callback)
    {
        on_trigger_stay -= callback;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (on_trigger_stay != null)
            on_trigger_stay(collision);
    }

    // -------

    public void SuscribeOnCollisionEnter2D(DelOnCollision2D callback)
    {
        on_collision_enter_2d += callback;
    }

    public void UnSuscribeOnCollisionEnter2D(DelOnCollision2D callback)
    {
        on_collision_enter_2d -= callback;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (on_collision_enter_2d != null)
            on_collision_enter_2d(collision);
    }

    // -------

    public void SuscribeOnCollisionExit2D(DelOnCollision2D callback)
    {
        on_collision_enter_2d += callback;
    }

    public void UnSuscribeOnCollisionExit2D(DelOnCollision2D callback)
    {
        on_collision_enter_2d -= callback;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (on_collision_exit_2d != null)
            on_collision_exit_2d(collision);
    }

    // -------

    public void SuscribeOnCollisionStay2D(DelOnCollision2D callback)
    {
        on_collision_enter_2d += callback;
    }

    public void UnSuscribeOnCollisionStay2D(DelOnCollision2D callback)
    {
        on_collision_enter_2d -= callback;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (on_collision_stay_2d != null)
            on_collision_stay_2d(collision);
    }

    // -------

    public void SuscribeOnTriggerEnter2D(DelOnTrigger2D callback)
    {
        on_trigger_enter_2d += callback;
    }

    public void UnSuscribeOnTriggerEnter2D(DelOnTrigger2D callback)
    {
        on_trigger_enter_2d -= callback;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (on_trigger_enter_2d != null)
            on_trigger_enter_2d(collision);
    }

    // -------

    public void SuscribeOnTriggerExit2D(DelOnTrigger2D callback)
    {
        on_trigger_exit_2d += callback;
    }

    public void UnSuscribeOnTriggerExit2D(DelOnTrigger2D callback)
    {
        on_trigger_exit_2d -= callback;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (on_trigger_exit_2d != null)
            on_trigger_exit_2d(collision);
    }

    // -------

    public void SuscribeOnTriggerStay2D(DelOnTrigger2D callback)
    {
        on_trigger_stay_2d += callback;
    }

    public void UnSuscribeOnTriggerStay2D(DelOnTrigger2D callback)
    {
        on_trigger_stay_2d -= callback;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (on_trigger_stay_2d != null)
            on_trigger_stay_2d(collision);
    }

    private DelOnCollision on_collision_enter = null;
    private DelOnCollision on_collision_exit = null;
    private DelOnCollision on_collision_stay = null;
    private DelOnTrigger on_trigger_enter = null;
    private DelOnTrigger on_trigger_exit = null;
    private DelOnTrigger on_trigger_stay = null;
    private DelOnCollision2D on_collision_enter_2d = null;
    private DelOnCollision2D on_collision_exit_2d = null;
    private DelOnCollision2D on_collision_stay_2d = null;
    private DelOnTrigger2D on_trigger_enter_2d = null;
    private DelOnTrigger2D on_trigger_exit_2d = null;
    private DelOnTrigger2D on_trigger_stay_2d = null;
}
