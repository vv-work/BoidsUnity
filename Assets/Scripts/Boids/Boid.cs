
using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Boid {
    Vector position;
    Vector velocity;
    Vector acceleration;

    static int fieldOfView = 120;

    static int size = 3;
    private static List<Vector2> shape;

    //todo: Make shape of triangle
    /*
    static {
        shape.moveTo(0,-size*2);
        shape.lineTo(-size, size*2);
        shape.lineTo(size,size*2);
        shape.closePath();
    }
    */
    
    static bool hasInfected = false;
    bool hasDisease = false;
    Color healthStatus = HEALTHY;
    float immunity = ((UnityEngine.Random.Range(0,1)*10+5));
    private float immunityCap ;//= immunity;
    static float lifeSpan = (UnityEngine.Random.Range(0,1)*300+500)*2;
    float initialLifeSpan = lifeSpan;
    bool dead = false, diagnosed = false;
    float deathAngle = 0;
    static int mortalityRate = 14;
    static Color RECOVERED = new Color(101,194,255), DEAD = new Color(154, 74, 178), 
                HEALTHY = Color.white, INFECTED = Color.red,  PARANOID = new Color(174,243,177);
    Color PARAMEDIC = Color.blue, DIAGNOSED = new Color(134, 0 , 0);
    float immunityLife;
    bool isImmune = false, isParamedic = false;
    static Boid patient = null; static bool lockedOn = false;

    private float healTime;//= this.initialImmunity;

    int sirens = 0, sirenCount = 0;
    static int travelTime = 0;
    static int patientBlink = 0, patientBlinkCount = 0;
    static Sound siren = null;

    public Boid() {
        if(!hasInfected) {
            healthStatus = INFECTED;
            hasInfected = true;
            hasDisease = true;
            lifeSpan = 2000;
        }
        this.position = new Vector(((UnityEngine.Random.Range(0,1)*BoidRunner.WIDTH),(float)((UnityEngine.Random.Range(0,1)*BoidRunner.HEIGHT));
        float angle = (UnityEngine.Random.Range(0,1)*360;
        float radius = (UnityEngine.Random.Range(0,1)*2+2; //2-4
        this.velocity = new Vector((radius * Mathf.cos(angle)), (radius * Mathf.sin(angle)));
        this.acceleration = new Vector(0,0);
        if((int)((UnityEngine.Random.Range(0,1)*500)==0 && !hasDisease) {
            this.isParamedic = true;
            this.healthStatus = PARAMEDIC;
            immunity = 2000;
        }
    }
    
    public Boid(int mouseXPosition, int mouseYPosition, bool addedInfected) {
        if(addedInfected) {
            healthStatus = INFECTED;
            hasInfected = true;
            hasDisease = true;
        }
        this.position = new Vector(mouseXPosition, mouseYPosition);
        float angle = (UnityEngine.Random.Range(0,1)*360;
        float radius = (UnityEngine.Random.Range(0,1)*2+2;
        this.velocity = new Vector((radius * Mathf.cos(angle)), (radius * Mathf.sin(angle)));
        this.acceleration = new Vector(0,0);
        if(BoidRunner.totalInfected == 1)
            this.lifeSpan = 12000;
    }
    public Boid(bool addedParamedic) {
        this.position = new Vector((int)(BoidRunner.WIDTH), (int)(BoidRunner.HEIGHT));
        float angle = (UnityEngine.Random.Range(0,1)*360;
        float radius = (UnityEngine.Random.Range(0,1)*2+2;
        this.velocity = new Vector((radius * Mathf.cos(angle)), (radius * Mathf.sin(angle)));
        this.acceleration = new Vector(0,0);
        if(addedParamedic) {
            this.isParamedic = true;
            this.healthStatus = PARAMEDIC;
            immunity = 500;
        }
    }

    Vector align(ArrayList<Boid> flock) {
        int perceptionRadius = (int)(alignmentPerceptionRadius);
        int total = 0;
        Vector steering = new Vector(0,0);
        //Part 2: Lifespans
        if(this.hasDisease && !this.dead && !this.isImmune) {
            lifeSpan--;
            if(lifeSpan <= 0) {
                if((int)((UnityEngine.Random.Range(0,1)*100) < mortalityRate) {
                    this.dead = true; //Death
                    BoidRunner.updateDead();
                    this.healthStatus = DEAD;
                } else {
                    this.hasDisease = false; //Recovery
                    this.isImmune = true;
                    if(this.diagnosed) {
                        patient = null;
                        lockedOn = false;
                    }
                    new Sound("recovery.wav");
                    this.healthStatus = RECOVERED;
                    this.immunity = this.immunityCap * ((UnityEngine.Random.Range(0,1)*50+100);
                    this.immunityCap = this.immunity;
                    this.immunityLife = initialLifeSpan*(6*((UnityEngine.Random.Range(0,1)*0.8+0.5));
                    if(this.diagnosed) {
                        this.diagnosed = false;
                        if(this == patient) {
                            Boid.travelTime = 0;
                            siren.stopSong();
                            siren = null;
                        }
                    }
                }
            }
        } else if(this.isImmune) { //Immunity loss
            this.immunityLife--;
            if(this.immunityLife < 0) {
                this.isImmune = false;
                this.healthStatus = HEALTHY;
                this.immunity = this.initialImmunity*((UnityEngine.Random.Range(0,1)*0.8+0.4);
                this.immunityCap = this.immunity;
                this.immunityLife = initialLifeSpan*(6*((UnityEngine.Random.Range(0,1)*0.8+0.5));
                this.lifeSpan = this.initialLifeSpan;
                new Sound("immunitylost.wav");
            }
        } //Alignment
        if(!this.isParamedic || (this.isParamedic && !lockedOn)) 
        for(int i = 0; i < flock.size(); i++) {
            if(this.isParamedic && flock[i].diagnosed) { //Lock on
                patient = flock[i];
                lockedOn = true;
                if(siren==null)
                    switch((int)((UnityEngine.Random.Range(0,1)*3)){
                        case 0:
                            siren = new Sound("ambulance.wav");
                            break;
                        case 1:
                            siren = new Sound("ambulance2.wav");
                            break; 
                        case 2:
                            siren = new Sound("ambulance3.wav");
                            break;
                    }
                break;
            }
            float dist = distance(this.position.xvalue, this.position.yvalue, flock[i].position.xvalue, flock[i].position.yvalue);
            if(flock[i] != this && dist < perceptionRadius) {
                if(!(this.diagnosed && flock[i].isParamedic)) {
                    steering.add(flock[i].velocity);
                    total++;
                }
                //!Viral transmission
                if(this.hasDisease && !flock[i].hasDisease && (!this.isImmune || flock[i].dead)) {
                    if(flock[i].immunity <= 0) {
                        if(flock[i].healthStatus == PARANOID)
                            new Sound("paranoiaEnded.wav");
                        flock[i].healthStatus = INFECTED; //!Infection
                        new Sound("newpatient.wav");
                        flock[i].hasDisease = true;
                        if(this.isParamedic) {
                            this.isParamedic = false;
                            new Sound("bell.wav");
                        }
                    }
                    else {//!Immunity loss
                        if((int)((UnityEngine.Random.Range(0,1)*40000)==0 && !this.diagnosed && !this.dead) { //prevent float diagnoses while diagnosed
                            this.healthStatus = DIAGNOSED; //!Diagnosis
                            this.diagnosed = true;
                            new Sound("diagnosis.wav");
                        }
                        flock[i].immunity -= (1/dist)*((BoidRunner.totalInfected > 35) ? 1 : ((BoidRunner.totalInfected > 11) 
                                                 ? 2.5 : ((BoidRunner.totalInfected < 5) ? (BoidRunner.totalInfected < 2 ? 5: 4) : 3.5)));
                    }
                } else if(!this.hasDisease && !flock[i].hasDisease && flock[i].immunity < flock[i].immunityCap && !flock[i].isImmune) {
                    flock[i].immunity += ((UnityEngine.Random.Range(0,1)*5+1)/((BoidRunner.totalInfected > 35) ? 10000 : 100);
                    if(flock[i].immunity > flock[i].immunityCap)
                       flock[i].immunity = flock[i].immunityCap; //!Immunity gain
                } if(flock[i].isParamedic && this.diagnosed && dist < 5) {
                    healTime--;
                    if(healTime <= 0) {
                        this.hasDisease = false; //!Paramedic Curing
                        this.isImmune = true;
                        this.diagnosed = false;
                        if(siren!=null) siren.stopSong();
                        siren = null;
                        new Sound("treatment.wav");
                        this.healthStatus = RECOVERED;
                        this.immunity = this.immunityCap * ((UnityEngine.Random.Range(0,1)*50+100);
                        this.immunityCap = this.immunity;
                        this.immunityLife = initialLifeSpan*(6*((UnityEngine.Random.Range(0,1)*0.8+0.5));
                        lockedOn = false;
                        patient = null;
                        Boid.travelTime = 0;
                    }
                }
                    
            }
        }
        if(total > 0) {
            if(total > 0)
                steering.divide((float)total);
            steering.setMagnitude(maxSpeed);
            steering.subtract(this.velocity);
            steering.limit(maxForce);
        }
        return steering;
    }

    Vector cohesion(ArrayList<Boid> flock) {
        int perceptionRadius = (int)(cohesionPerceptionRadius);
        int total = 0;
        Vector steering = new Vector(0,0);
        if(!this.isParamedic || (this.isParamedic && !lockedOn))
            for(Boid boid : flock) {
                float dist = distance(this.position.xvalue, this.position.yvalue, boid.position.xvalue, boid.position.yvalue);
                if(boid != this && dist < perceptionRadius) {
                    steering.add(boid.position);
                    total++;
                }
            }
        if((total > 0 || (this.isParamedic && lockedOn && patient.velocity.movement() != 0))) {
            if(total > 0)
                steering.divide((float)total);
            else {
                patientDistance = distance(this.position.xvalue, this.position.yvalue, patient.position.xvalue, patient.position.yvalue);
                steering.add(patient.position);
            }
            steering.subtract(this.position);
            steering.setMagnitude(maxSpeed);
            steering.subtract(this.velocity);
            steering.limit(maxForce*((this.isParamedic && lockedOn)?3:1));
        }
        return steering;
    }

    Vector separation(ArrayList<Boid> flock) {
        int perceptionRadius = (int)separationPerceptionRadius;
        int total = 0;
        Vector steering = new Vector(0,0);
        bool emergencyServicePresent = false;
        for(Boid boid : flock) {
            float dist = distance(this.position.xvalue, this.position.yvalue, boid.position.xvalue, boid.position.yvalue);
            if(boid != this && dist < perceptionRadius && !(this.diagnosed && boid.isParamedic)) {
                Vector difference = new Vector(this.position.xvalue, this.position.yvalue);
                difference.subtract(boid.position);
                if(dist == 0.0) dist += 0.001;
                difference.divide(dist*dist);
                if((boid.dead || (boid.diagnosed && !this.isParamedic) || this.healthStatus == PARANOID || (boid.isParamedic && lockedOn)) && !this.isParamedic){
                    difference.multiply((UnityEngine.Random.Range(0,1)*5+((boid.isParamedic && lockedOn)?80:20));
                } if(this.isParamedic && boid.isParamedic && lockedOn 
                        && distance(this.position.xvalue, this.position.yvalue, patient.position.xvalue, boid.position.yvalue) > 150 && dist < 5) {
                    difference.multiply(15);
                }
                if(boid.isParamedic && lockedOn && !this.isParamedic)
                    emergencyServicePresent = true;
                steering.add(difference);
                total++;
            }
        }
        if(total > 0) {
            steering.divide((float)total);
            steering.setMagnitude(((total > 40 || emergencyServicePresent) ? separationMaxSpeed
                    *((emergencyServicePresent)?6:2) : ((this.healthStatus == PARANOID)? separationMaxSpeed*5:separationMaxSpeed)));
            steering.subtract(this.velocity);
            steering.limit(((total > 40 || emergencyServicePresent) ? separationMaxForce
                    *((emergencyServicePresent)?6:2) : ((this.healthStatus == PARANOID)? separationMaxForce*5:separationMaxForce)));
        }
        return steering;
    }

    void flock(ArrayList<Boid> flock) {
        bool emergencyWork = false;
        if(this.isParamedic && lockedOn)
            emergencyWork = true;
        this.acceleration.set(0, 0);
        Vector alignment = this.align(flock);
        Vector cohesion = this.cohesion(flock);
        Vector separation = this.separation(flock);
        //Force accumulation:
        if(!emergencyWork) 
            this.acceleration.add(alignment);
        this.acceleration.add(separation);
        this.acceleration.add(cohesion);
    }

    float patientDistance;

    void update() {
        if(!this.dead) {
            if(this.isParamedic && lockedOn && patientDistance >= 10) {
                if((int)((UnityEngine.Random.Range(0,1)*BoidRunner.paramedicCount) == 0) //since travelTime is static and you only want to increase it by
                     Boid.travelTime++;       //about one every cycle, have it be a 1/paramedicCount chance for the traveltime to increase
                Vector emergencyVelocity = this.velocity.setMagnitude(
                    this.velocity.getMagnitude()*2+((Boid.travelTime > 20)?Boid.travelTime/200:1));
                this.position.add(emergencyVelocity); 
            }
            else
                this.position.add(this.velocity);
        }
        this.velocity.add(this.acceleration);
        this.velocity.limit(maxSpeed);
        if(this.dead && deathAngle == 0) {
            deathAngle = this.velocity.dir() + Mathf.PI/2;
        }
        if(patient == this && lockedOn) {
            patientBlinkCount++;
            if(patientBlinkCount % 4 == 0) {
                patientBlink++;
                switch(patientBlink) {
                    case 0 :
                        this.DIAGNOSED = new Color(252, 52, 52);
                        break;
                    case 1 :
                        this.DIAGNOSED = new Color(134, 0 , 0);
                        break;
                } patient.healthStatus = this.DIAGNOSED;
                if(patientBlink > 1) patientBlink = -1;
            }
        }
        //Ensures that paramedics do not treat a diagnosed Boid turned dead
        if(this.isParamedic && lockedOn && patient.dead) {
            patient.diagnosed = false;
            siren.stopSong();
            siren = null;
            lockedOn = false;
            patient = null;
            Boid.travelTime = 0;
        }
    }

    void edges() {
        if(this.position.xvalue > BoidRunner.WIDTH)
            this.position.xvalue = 0;
        else if(this.position.xvalue < 0)
            this.position.xvalue = BoidRunner.WIDTH;
        
        if(this.position.yvalue > BoidRunner.HEIGHT)
            this.position.yvalue = 0;
        else if(this.position.yvalue < 0)
            this.position.yvalue = BoidRunner.HEIGHT;
    }

    float distance(float x1, float y1, float x2, float y2) {
        return Mathf.Sqrt(Mathf.Pow((x2-x1), 2) + Mathf.Pow((y2-y1), 2));
    }

    public void draw(Graphics2D g) {
        AffineTransform save = g.getTransform();
        g.translate((int)this.position.xvalue, (int)this.position.yvalue);
        if(!this.dead)
            g.rotate(this.velocity.dir() + Mathf.PI/2);
        else
            g.rotate(deathAngle);
        g.setColor(healthStatus);
        g.fill(shape);
        g.draw(shape);
        g.setTransform(save);
    }

    public static void pause() {
        try{
            Thread.sleep(3000);
        } catch(InterruptedException e) {}
    }

    static float maxForce = 0.2;
    static float maxSpeed = 2;

    static readonly float forceChangeValue = 1;

    static float alignmentPerceptionRadius = 50;
    static float cohesionPerceptionRadius = 100;
    static float separationPerceptionRadius = 100;
    static float separationMaxSpeed = maxSpeed;
    static float separationMaxForce = maxForce;
    
    static void incrementSeparationMaxForce() { Boid.separationMaxForce += forceChangeValue; }
    static void decrementSeparationMaxForce() { Boid.separationMaxForce -= forceChangeValue; }
}