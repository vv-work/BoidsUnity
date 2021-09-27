
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
    
    public float getXValue() { return xvalue; }
    public float getYValue() { return yvalue; }
        
    public void setXValue(float newValue) { xvalue = newValue; }
    public void setYValue(float newValue) { yvalue = newValue; }

    public float getMagnitude() {
        return Mathf.Sqrt(Mathf.Pow(xvalue, 2) + Mathf.Pow(yvalue, 2));
    }

    public void limit(float maxForce) {
        float magnitude = Mathf.Sqrt(Mathf.Pow(xvalue, 2) + Mathf.Pow(yvalue, 2));
        float multiplier;
        if(magnitude > maxForce) 
            multiplier = maxForce / magnitude;
        else
            multiplier = 1f;
        
        xvalue *= multiplier;
        yvalue *= multiplier;
    }

    public Vector setMagnitude(float newMagnitude) {
        float currentMagnitude = Mathf.Sqrt(Mathf.Pow(xvalue, 2) + Mathf.Pow(yvalue, 2));
        xvalue *= (newMagnitude/currentMagnitude);
        yvalue *= (newMagnitude/currentMagnitude);
        return this;
    }

    public void add(Vector parent) {
        xvalue += parent.getXValue();
        yvalue += parent.getYValue();
    }

    public void subtract(Vector parent) {
        xvalue -= parent.getXValue();
        yvalue -= parent.getYValue();
    }

    void multiply(float multiplier) {
        xvalue *= multiplier;
        yvalue *= multiplier;
    }

    public void divide(float denominator) {
        xvalue /= denominator;
        yvalue /= denominator;
    }

    public float dir() {
        return Mathf.Atan2(yvalue, xvalue);
    }

    void setValues(float xvalue, float yvalue) {
        this.xvalue = xvalue;
        this.yvalue = yvalue;
    }

    float movement() {
        return xvalue+yvalue;
    }
}