using System;
using UnityEngine;

public class Vector {
    public float xvalue;
    public float yvalue;
    
    
    public Vector(float xvalue, float yvalue) {
        this.xvalue = xvalue;
        this.yvalue = yvalue;
    }


    public void set(float xvalue, float yvalue) {
        this.xvalue = xvalue;
        this.yvalue = yvalue;
    }
    
    public float getXValue() { return this.xvalue; }
    public float getYValue() { return this.yvalue; }
        
    public void setXValue(float newValue) { this.xvalue = newValue; }
    public void setYValue(float newValue) { this.yvalue = newValue; }

    public float getMagnitude() {
        return Mathf.Sqrt(Mathf.Pow(this.xvalue, 2) + Mathf.Pow(this.yvalue, 2));
    }

    public void limit(float maxForce) {
        float magnitude = Mathf.Sqrt(Mathf.Pow(this.xvalue, 2) + Mathf.Pow(this.yvalue, 2));
        float multiplier;
        if(magnitude > maxForce) 
            multiplier = maxForce / magnitude;
        else
            multiplier = 1f;
        
        this.xvalue *= multiplier;
        this.yvalue *= multiplier;
    }

    public Vector setMagnitude(float newMagnitude) {
        float currentMagnitude = Mathf.Sqrt(Mathf.Pow(this.xvalue, 2) + Mathf.Pow(this.yvalue, 2));
        this.xvalue *= (newMagnitude/currentMagnitude);
        this.yvalue *= (newMagnitude/currentMagnitude);
        return this;
    }
    
    void add(Vector parent) {
        this.xvalue += parent.getXValue();
        this.yvalue += parent.getYValue();
    }

    void subtract(Vector parent) {
        this.xvalue -= parent.getXValue();
        this.yvalue -= parent.getYValue();
    }

    void multiply(float multiplier) {
        this.xvalue *= multiplier;
        this.yvalue *= multiplier;
    }

    void divide(float denominator) {
        this.xvalue /= denominator;
        this.yvalue /= denominator;
    }

    float dir() {
        return Mathf.Atan2(this.yvalue, this.xvalue);
    }

    void setValues(float xvalue, float yvalue) {
        this.xvalue = xvalue;
        this.yvalue = yvalue;
    }

    float movement() {
        return this.xvalue+this.yvalue;
    }
}