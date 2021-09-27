
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Scripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Boid {
    Vector _position;
    Vector _velocity;
    Vector _acceleration;

    static int _fieldOfView = 120;

    static int _size = 3;
    private static List<Vector2> _shape;

    //todo: Make shape of triangle
    /*
    static {
        shape.moveTo(0,-size*2);
        shape.lineTo(-size, size*2);
        shape.lineTo(size,size*2);
        shape.closePath();
    }
    */

    public static bool _hasInfected = false;
    public bool _hasDisease = false;
    public Color _healthStatus = _healthy;
    float _immunity = ((UnityEngine.Random.Range(0,1)*10+5));
    private float _immunityCap ;//= immunity;
    static float _lifeSpan = (UnityEngine.Random.Range(0,1)*300+500)*2;
    float _initialLifeSpan = _lifeSpan;
    public bool IsDead { get; private set; }
    public bool  IsDiagnosed { get; private set; }
    float _deathAngle = 0;
    static int _mortalityRate = 14;

    public static Color _recovered = new Color(101,194,255);
    static Color _dead = new Color(154, 74, 178);

    public static Color _healthy = Color.white;
    public static Color _infected = Color.red;
    public static Color _paranoid = new Color(174,243,177);
    public Color _paramedic = Color.blue;
    Color _diagnosed = new Color(134, 0 , 0);
    float _immunityLife;
    bool _isImmune = false;
    public bool _isParamedic = false;
    static Boid _patient = null;
    public static bool _lockedOn = false;

    private float _healTime;//= this.initialImmunity;

    public int _sirens = 0;
    public int _sirenCount = 0;
    static int _travelTime = 0;
    static int _patientBlink = 0, _patientBlinkCount = 0;
    static Sound _siren = null;

    public Boid() {
        if(!_hasInfected) {
            _healthStatus = _infected;
            _hasInfected = true;
            _hasDisease = true;
            _lifeSpan = 2000;
        }
        this._position = new Vector(xvalue: (UnityEngine.Random.Range(0,1)*BoidRunner.WIDTH),(float)(UnityEngine.Random.Range(0,1)*BoidRunner.HEIGHT));
        float angle = (UnityEngine.Random.Range(0,1)*360f);
        float radius = (UnityEngine.Random.Range(0,1)*2+2f); //2-4
        this._velocity = new Vector((radius * Mathf.cos(angle)), (radius * Mathf.sin(angle)));
        this._acceleration = new Vector(0,0);
        if((int)((UnityEngine.Random.Range(0,1)*500)==0 && !_hasDisease) {
            this._isParamedic = true;
            this._healthStatus = _paramedic;
            _immunity = 2000;
        }
    }
    
    public Boid(int mouseXPosition, int mouseYPosition, bool addedInfected) {
        if(addedInfected) {
            _healthStatus = _infected;
            _hasInfected = true;
            _hasDisease = true;
        }
        this._position = new Vector(mouseXPosition, mouseYPosition);
        float angle = (UnityEngine.Random.Range(0,1)*360;
        float radius = (UnityEngine.Random.Range(0,1)*2+2;
        this._velocity = new Vector((radius * Mathf.cos(angle)), (radius * Mathf.sin(angle)));
        this._acceleration = new Vector(0,0);
        if(BoidRunner.totalInfected == 1)
            this._lifeSpan = 12000;
    }
    public Boid(bool addedParamedic) {
        this._position = new Vector((int)(BoidRunner.WIDTH), (int)(BoidRunner.HEIGHT));
        float angle = (UnityEngine.Random.Range(0,1)*360;
        float radius = (UnityEngine.Random.Range(0,1)*2+2;
        this._velocity = new Vector((radius * Mathf.cos(angle)), (radius * Mathf.sin(angle)));
        this._acceleration = new Vector(0,0);
        if(addedParamedic) {
            this._isParamedic = true;
            this._healthStatus = _paramedic;
            _immunity = 500;
        }
    }

    Vector Align(List<Boid> flock) {
        int perceptionRadius = (int)(_alignmentPerceptionRadius);
        int total = 0;
        Vector steering = new Vector(0,0);
        //Part 2: Lifespans
        if(this._hasDisease && !this._dead && !this._isImmune) {
            _lifeSpan--;
            if(_lifeSpan <= 0) {
                if((int)((UnityEngine.Random.Range(0,1)*100) < _mortalityRate) {
                    this._dead = true; //Death
                    BoidRunner.updateDead();
                    this._healthStatus = DEAD;
                } else {
                    this._hasDisease = false; //Recovery
                    this._isImmune = true;
                    if(this._diagnosed) {
                        _patient = null;
                        _lockedOn = false;
                    }
                    new Sound("recovery.wav");
                    this._healthStatus = _recovered;
                    this._immunity = this._immunityCap * ((UnityEngine.Random.Range(0,1)*50+100);
                    this._immunityCap = this._immunity;
                    this._immunityLife = _initialLifeSpan*(6*((UnityEngine.Random.Range(0,1)*0.8+0.5));
                    if(this._diagnosed) {
                        this._diagnosed = false;
                        if(this == _patient) {
                            Boid._travelTime = 0;
                            _siren.stopSong();
                            _siren = null;
                        }
                    }
                }
            }
        } else if(this._isImmune) { //Immunity loss
            this._immunityLife--;
            if(this._immunityLife < 0) {
                this._isImmune = false;
                this._healthStatus = _healthy;
                this._immunity = this.initialImmunity*((UnityEngine.Random.Range(0,1)*0.8+0.4);
                this._immunityCap = this._immunity;
                this._immunityLife = _initialLifeSpan*(6*((UnityEngine.Random.Range(0,1)*0.8+0.5));
                this._lifeSpan = this._initialLifeSpan;
                new Sound("immunitylost.wav");
            }
        } //Alignment
        if(!this._isParamedic || (this._isParamedic && !_lockedOn)) 
        for(int i = 0; i < flock.Count; i++) {
            if(this._isParamedic && flock[i].diagnosed) { //Lock on
                _patient = flock[i];
                _lockedOn = true;
                if(_siren==null)
                    switch((int)((UnityEngine.Random.Range(0,1)*3)){
                        case 0:
                            _siren = new Sound("ambulance.wav");
                            break;
                        case 1:
                            _siren = new Sound("ambulance2.wav");
                            break; 
                        case 2:
                            _siren = new Sound("ambulance3.wav");
                            break;
                    }
                break;
            }
            float dist = Distance(this._position.xvalue, this._position.yvalue, flock[i].position.xvalue, flock[i].position.yvalue);
            if(flock[i] != this && dist < perceptionRadius) {
                if(!(this._diagnosed && flock[i].isParamedic)) {
                    steering.add(flock[i].velocity);
                    total++;
                }
                //!Viral transmission
                if(this._hasDisease && !flock[i].hasDisease && (!this._isImmune || flock[i].dead)) {
                    if(flock[i].immunity <= 0) {
                        if(flock[i].healthStatus == _paranoid)
                            new Sound("paranoiaEnded.wav");
                        flock[i].healthStatus = _infected; //!Infection
                        new Sound("newpatient.wav");
                        flock[i].hasDisease = true;
                        if(this._isParamedic) {
                            this._isParamedic = false;
                            new Sound("bell.wav");
                        }
                    }
                    else {//!Immunity loss
                        if((int)((UnityEngine.Random.Range(0,1)*40000)==0 && !this._diagnosed && !this._dead) { //prevent float diagnoses while diagnosed
                            this._healthStatus = DIAGNOSED; //!Diagnosis
                            this._diagnosed = true;
                            new Sound("diagnosis.wav");
                        }
                        flock[i].immunity -= (1/dist)*((BoidRunner.totalInfected > 35) ? 1 : ((BoidRunner.totalInfected > 11) 
                                                 ? 2.5 : ((BoidRunner.totalInfected < 5) ? (BoidRunner.totalInfected < 2 ? 5: 4) : 3.5)));
                    }
                } else if(!this._hasDisease && !flock[i].hasDisease && flock[i].immunity < flock[i].immunityCap && !flock[i].isImmune) {
                    flock[i].immunity += ((UnityEngine.Random.Range(0,1)*5+1)/((BoidRunner.totalInfected > 35) ? 10000 : 100);
                    if(flock[i].immunity > flock[i].immunityCap)
                       flock[i].immunity = flock[i].immunityCap; //!Immunity gain
                } if(flock[i].isParamedic && this._diagnosed && dist < 5) {
                    _healTime--;
                    if(_healTime <= 0) {
                        this._hasDisease = false; //!Paramedic Curing
                        this._isImmune = true;
                        this._diagnosed = false;
                        if(_siren!=null) _siren.stopSong();
                        _siren = null;
                        new Sound("treatment.wav");
                        this._healthStatus = _recovered;
                        this._immunity = this._immunityCap * ((UnityEngine.Random.Range(0,1)*50+100);
                        this._immunityCap = this._immunity;
                        this._immunityLife = _initialLifeSpan*(6*((UnityEngine.Random.Range(0,1)*0.8+0.5));
                        _lockedOn = false;
                        _patient = null;
                        Boid._travelTime = 0;
                    }
                }
                    
            }
        }
        if(total > 0) {
            if(total > 0)
                steering.divide((float)total);
            steering.setMagnitude(_maxSpeed);
            steering.subtract(this._velocity);
            steering.limit(_maxForce);
        }
        return steering;
    }

    Vector Cohesion(List<Boid> flock) {
        int perceptionRadius = (int)(_cohesionPerceptionRadius);
        int total = 0;
        Vector steering = new Vector(0,0);
        if(!this._isParamedic || (this._isParamedic && !_lockedOn))
            for(Boid boid : flock) {
                float dist = Distance(this._position.xvalue, this._position.yvalue, boid.position.xvalue, boid.position.yvalue);
                if(boid != this && dist < perceptionRadius) {
                    steering.add(boid.position);
                    total++;
                }
            }
        if((total > 0 || (this._isParamedic && _lockedOn && _patient._velocity.movement() != 0))) {
            if(total > 0)
                steering.divide((float)total);
            else {
                _patientDistance = Distance(this._position.xvalue, this._position.yvalue, _patient._position.xvalue, _patient._position.yvalue);
                steering.add(_patient._position);
            }
            steering.subtract(this._position);
            steering.setMagnitude(_maxSpeed);
            steering.subtract(this._velocity);
            steering.limit(_maxForce*((this._isParamedic && _lockedOn)?3:1));
        }
        return steering;
    }

    Vector Separation(List<Boid> flock) {
        int perceptionRadius = (int)_separationPerceptionRadius;
        int total = 0;
        Vector steering = new Vector(0,0);
        bool emergencyServicePresent = false;
        for(Boid boid : flock) {
            float dist = Distance(this._position.xvalue, this._position.yvalue, boid.position.xvalue, boid.position.yvalue);
            if(boid != this && dist < perceptionRadius && !(this._diagnosed && boid.isParamedic)) {
                Vector difference = new Vector(this._position.xvalue, this._position.yvalue);
                difference.subtract(boid.position);
                if(dist == 0.0) dist += 0.001;
                difference.divide(dist*dist);
                if((boid.dead || (boid.diagnosed && !this._isParamedic) || this._healthStatus == _paranoid || (boid.isParamedic && _lockedOn)) && !this._isParamedic){
                    difference.multiply((UnityEngine.Random.Range(0,1)*5+((boid.isParamedic && _lockedOn)?80:20));
                } if(this._isParamedic && boid.isParamedic && _lockedOn 
                        && Distance(this._position.xvalue, this._position.yvalue, _patient._position.xvalue, boid.position.yvalue) > 150 && dist < 5) {
                    difference.multiply(15);
                }
                if(boid.isParamedic && _lockedOn && !this._isParamedic)
                    emergencyServicePresent = true;
                steering.add(difference);
                total++;
            }
        }
        if(total > 0) {
            steering.divide((float)total);
            steering.setMagnitude(((total > 40 || emergencyServicePresent) ? _separationMaxSpeed
                    *((emergencyServicePresent)?6:2) : ((this._healthStatus == _paranoid)? _separationMaxSpeed*5:_separationMaxSpeed)));
            steering.subtract(this._velocity);
            steering.limit(((total > 40 || emergencyServicePresent) ? _separationMaxForce
                    *((emergencyServicePresent)?6:2) : ((this._healthStatus == _paranoid)? _separationMaxForce*5:_separationMaxForce)));
        }
        return steering;
    }

    public void Flock(List<Boid> flock) {
        bool emergencyWork = false;
        if(this._isParamedic && _lockedOn)
            emergencyWork = true;
        this._acceleration.set(0, 0);
        Vector alignment = this.Align(flock);
        Vector cohesion = this.Cohesion(flock);
        Vector separation = this.Separation(flock);
        //Force accumulation:
        if(!emergencyWork) 
            this._acceleration.add(alignment);
        this._acceleration.add(separation);
        this._acceleration.add(cohesion);
    }

    float _patientDistance;

    public void Update() {
        if(!this._dead) {
            if(this._isParamedic && _lockedOn && _patientDistance >= 10) {
                if((int)((UnityEngine.Random.Range(0,1)*BoidRunner.paramedicCount) == 0) //since travelTime is static and you only want to increase it by
                     Boid._travelTime++;       //about one every cycle, have it be a 1/paramedicCount chance for the traveltime to increase
                Vector emergencyVelocity = this._velocity.setMagnitude(
                    this._velocity.getMagnitude()*2+((Boid._travelTime > 20)?Boid._travelTime/200:1));
                this._position.add(emergencyVelocity); 
            }
            else
                this._position.add(this._velocity);
        }
        this._velocity.add(this._acceleration);
        this._velocity.limit(_maxSpeed);
        if(this._dead && _deathAngle == 0) {
            _deathAngle = this._velocity.dir() + Mathf.PI/2;
        }
        if(_patient == this && _lockedOn) {
            _patientBlinkCount++;
            if(_patientBlinkCount % 4 == 0) {
                _patientBlink++;
                switch(_patientBlink) {
                    case 0 :
                        this.DIAGNOSED = new Color(252, 52, 52);
                        break;
                    case 1 :
                        this.DIAGNOSED = new Color(134, 0 , 0);
                        break;
                } _patient._healthStatus = this.DIAGNOSED;
                if(_patientBlink > 1) _patientBlink = -1;
            }
        }
        //Ensures that paramedics do not treat a diagnosed Boid turned dead
        if(this._isParamedic && _lockedOn && _patient._dead) {
            _patient._diagnosed = false;
            _siren.stopSong();
            _siren = null;
            _lockedOn = false;
            _patient = null;
            Boid._travelTime = 0;
        }
    }

    public void Edges() {
        if(this._position.xvalue > BoidRunner.WIDTH)
            this._position.xvalue = 0;
        else if(this._position.xvalue < 0)
            this._position.xvalue = BoidRunner.WIDTH;
        
        if(this._position.yvalue > BoidRunner.HEIGHT)
            this._position.yvalue = 0;
        else if(this._position.yvalue < 0)
            this._position.yvalue = BoidRunner.HEIGHT;
    }

    float Distance(float x1, float y1, float x2, float y2) {
        return Mathf.Sqrt(Mathf.Pow((x2-x1), 2) + Mathf.Pow((y2-y1), 2));
    }

    public void Draw<T>(T g) {
        //todo:Work with Method Draw
        /*
        AffineTransform save = g.getTransform();
        g.translate((int)this._position.xvalue, (int)this._position.yvalue);
        if(!this._dead)
            g.rotate(this._velocity.dir() + Mathf.PI/2);
        else
            g.rotate(_deathAngle);
        g.setColor(_healthStatus);
        g.fill(_shape);
        g.draw(_shape);
        g.setTransform(save);
        */
    }

    public static void Pause() {
        try{
            Thread.Sleep(3000);
        } catch(Exception e) {}
    }

    static float _maxForce = 0.2f;
    static float _maxSpeed = 2;

    static readonly float ForceChangeValue = 1;

    static float _alignmentPerceptionRadius = 50;
    static float _cohesionPerceptionRadius = 100;
    static float _separationPerceptionRadius = 100;
    static float _separationMaxSpeed = _maxSpeed;
    static float _separationMaxForce = _maxForce;
    public Color DIAGNOSED;

    static void IncrementSeparationMaxForce() { Boid._separationMaxForce += ForceChangeValue; }
    static void DecrementSeparationMaxForce() { Boid._separationMaxForce -= ForceChangeValue; }
}
