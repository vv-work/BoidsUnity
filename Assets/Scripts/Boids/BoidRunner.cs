
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BoidRunner{ //extends JPanel implements KeyListener, MouseListener, MouseMotionListener  {

//    private static readonly long serialVersionUID = -8716187417647724411L;
    
    public static readonly int BOIDCOUNT = 1200; //*Adjust this value to match your computer's optimal processing
    
    public static readonly int WIDTH = 1920;
    public static readonly int HEIGHT = 1080;

    static List<Boid> flock = new List<Boid>();
    static int totalInfected = 1, deathCount = 0, healthyCount = 0, criticalCount = 0, 
            aliveCount, recoveryCount = 0, visiblyDead = 0, diagnosedCount = 0, paramedicCount = 0, paranoidCount = 0;

    static Sprite infectedDisplay, deathDisplay, healthyDisplay, criticalDisplay, aliveDisplay, recoveredDisplay;
    private Sound music;
    
    public bool addedNewBoid = false;
    int mouseXPosition = (int)(WIDTH/2), mouseYPosition = (int)(HEIGHT/2);

    public BoidRunner() {
        //todo: Make Layout setter
        /*
        this.setLayout(null);
        this.setBackground(Color.BLACK);
        this.setPreferredSize(new Dimension(WIDTH, HEIGHT));
        this.setFocusable(true);

        this.addKeyListener(this);
        this.addMouseListener(this);

        createLabels();

        for(int i = 0; i < BOIDCOUNT; i++)
            flock.add(new Boid());

        music = new Sound("plague.wav");
    }

    @Override
    public void paintComponent(Graphics page) {
        super.paintComponent(page);
        Graphics2D g = (Graphics2D) page;
        g.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
        for(Boid boid: flock) {
            boid.draw(g);
        }
        */
    }

    bool intensityPlayed = false, milestonePlayed = false;

    public void run() {
        while(true) {
            int toAdd = 0;
            totalInfected = 0; healthyCount = 0; recoveryCount = 0; visiblyDead = 0; diagnosedCount = 0; paramedicCount = 0; paranoidCount = 0;
            for(int i = 0; i < flock.Count; i++){
                flock[i].Edges();
                flock[i].Flock(flock);
                flock[i].Update();
                if(flock[i]._isParamedic)
                    paramedicCount++;
                else if(flock[i]._healthStatus == Boid._healthy)
                    healthyCount++;
                else if(flock[i]._healthStatus == Boid._infected)
                    totalInfected++;
                else if(flock[i]._healthStatus == Boid._recovered)
                    recoveryCount++;
                else if(flock[i]._healthStatus == flock[i].DIAGNOSED)
                    diagnosedCount++;
                else if(flock[i]._healthStatus == Boid._paranoid)
                    paranoidCount++;
                else
                    visiblyDead++;
                if(flock[i].IsDead && (((UnityEngine.Random.Range(0,1)*(totalInfected*600+((totalInfected == 0)?1:0))) <= visiblyDead))) {
                    flock.RemoveAt(i);
                    i--;
                    //toAdd++;
                } else if(flock[i]._isParamedic && totalInfected <= flock.Count*0.25 && ((UnityEngine.Random.Range(0,1)*10000*(flock.Count-totalInfected)) == 0)) {
                    flock.RemoveAt(i);
                    i--;
                    //toAdd++;
                    new Sound("bell.wav");
                }
                else if(flock[i]._isParamedic && Boid._lockedOn) {
                    flock[i]._sirenCount++;
                    if(flock[i]._sirenCount % 3 == 0) {
                        flock[i]._sirens++;
                        if(flock[i]._sirens==0)
                            flock[i]._paramedic = Color.BLUE;
                        else if(flock[i]._sirens==1)
                            flock[i]._paramedic = Color.WHITE;
                        else if(flock[i]._sirens == 2)
                            flock[i]._paramedic = Color.RED;
                        flock[i]._healthStatus = flock[i]._paramedic;
                    } if(flock[i]._sirens > 2) flock[i]._sirens = -1;
                } else if(flock[i]._isParamedic && flock[i]._paramedic != Color.BLUE) {
                    flock[i]._paramedic = Color.BLUE;
                    flock[i]._healthStatus = flock[i]._paramedic;
                }
                if((int)((UnityEngine.Random.Range(0,1)*healthyCount*2000+((healthyCount == 0)?1:0)) == 0 && 
                        !flock[i]._hasDisease && diagnosedCount >= 3 && 
                        flock[i]._healthStatus != Boid._paranoid && paranoidCount <= 15) {
                    flock[i]._healthStatus = Boid._paranoid;
                    new Sound("paranoia.wav");
                } if(recoveryCount >= 800 && flock[i]._healthStatus == Boid._paranoid &&
                        (int)((UnityEngine.Random.Range(0,1)*totalInfected*200+((totalInfected == 0)?1:0)) == 0 ) {
                    flock[i]._healthStatus = Boid._healthy;
                    new Sound("paranoiaEnded.wav");
                }
            }
            if(clearGrid) {
                for(int i = 0; i < flock.Count; i++) {
                    flock.RemoveAt(i);
                    i--;
                }
                clearGrid = false;
            }
            if(addedBoids.Count != 0) {
                for(int i = 0; i < addedBoids.Count; i++) {
                    flock.add(addedBoids[i]);
                    addedBoids.RemoveAt(i);
                    i--;
                }
            }
            if(paramedicCount <= 2 && diagnosedCount != 0) {
                flock.add(new Boid(true));
                new Sound("ambulance.wav");
            }
            if(!intensityPlayed && (flock.Count+1)%100 == 0) 
                intensityPlayed = true;
            if(totalInfected == 0)
                flock.add(new Boid((int)((UnityEngine.Random.Range(0,1)*WIDTH), (int)((UnityEngine.Random.Range(0,1)*HEIGHT), true));
            else if(totalInfected >= (int)(flock.Count*0.8) && !intensityPlayed) {
                new Sound("intensity.wav");
                intensityPlayed = !intensityPlayed;
            }
            if(deathCount >= 100) {
                if(!milestonePlayed && deathCount % 100 == 0) {
                    new Sound("deathmilestone.wav");
                    milestonePlayed = true;
                } else if((deathCount-1)%100 == 0)
                    milestonePlayed = false;
            }
            updateValues();
            for(int i = 0; i < toAdd; i++)
                flock.add(new Boid());
            int more = (int)((UnityEngine.Random.Range(0,1)*((flock.Count>=900) ? 1000 : 500));
            if(more == 0)
                flock.add(new Boid());
            if(addedNewBoid) {
                flock.add(new Boid(mouseXPosition, mouseYPosition, false));
                addedNewBoid = false;
            }
            this.repaint();
            try {
                Thread.sleep(10);
            } catch( InterruptedException ex ){}
        }
    }

    void createLabels() {
        //Healthy
        healthyDisplay = new JLabel("Healthy: "+ healthyCount);
        this.setLayout(new FlowLayout());
        this.add(healthyDisplay);
        healthyDisplay.setFont(new Font("Courier New", Font.PLAIN, 20));
        healthyDisplay.setForeground(Color.WHITE);
        healthyDisplay.setVisible(true);
        healthyDisplay.setLocation((int)WIDTH/2-400, 200);
        //Infected
        infectedDisplay = new JLabel(" Infected: "+ totalInfected);
        this.setLayout(new FlowLayout());
        this.add(infectedDisplay);
        infectedDisplay.setFont(new Font("Courier New", Font.PLAIN, 20));
        infectedDisplay.setForeground(Color.RED);
        infectedDisplay.setVisible(true);
        infectedDisplay.setLocation((int)WIDTH/2, 200);
        //Recovered
        recoveredDisplay = new JLabel(" Recovered: "+ criticalCount);
        this.setLayout(new FlowLayout());
        this.add(recoveredDisplay);
        recoveredDisplay.setFont(new Font("Courier New", Font.PLAIN, 20));
        recoveredDisplay.setForeground(Boid._recovered);
        recoveredDisplay.setVisible(true);
        recoveredDisplay.setLocation((int)WIDTH/2+400, 200);
        //Death
        deathDisplay = new JLabel(" Dead: "+ deathCount);
        this.setLayout(new FlowLayout());
        this.add(deathDisplay);
        deathDisplay.setFont(new Font("Courier New", Font.PLAIN, 20));
        deathDisplay.setForeground(Boid.DEAD);
        deathDisplay.setVisible(true);
        deathDisplay.setLocation((int)WIDTH/2+200, 300);
    }

    static void toggleCounts(bool setting) {
        healthyDisplay.setVisible(setting);
        infectedDisplay.setVisible(setting);
        recoveredDisplay.setVisible(setting);
        deathDisplay.setVisible(setting);
    }

    static void updateValues() {
        healthyDisplay.setText("Healthy: " + healthyCount);
        infectedDisplay.setText(" Infected: " + totalInfected);
        recoveredDisplay.setText(" Recovered: " + recoveryCount);
        deathDisplay.setText(" Dead: " + deathCount);
    }

    static void updateHealthy() {
        healthyCount = flock.Count-totalInfected-deathCount;
        healthyDisplay.setText("Healthy: " + healthyCount);
    }

    static void updateInfected() {
        totalInfected++;
        healthyCount--;
        infectedDisplay.setText(" Infected: " + totalInfected);
        new Sound("newpatient.wav");
    }

    static void updateRecovered() {
        recoveryCount++;
        healthyCount++;
        totalInfected--;
        infectedDisplay.setText(" Infected: " + totalInfected);
        recoveredDisplay.setText(" Recovered: " + recoveryCount);
        new Sound("recovery.wav");
    }

    static void updateDead() {
        deathCount++;
        totalInfected--;
        infectedDisplay.setText(" Infected: " + totalInfected);
        deathDisplay.setText(" Dead: " + deathCount);
        new Sound("death.wav");
    }

    static void updateCritical() {
        criticalCount++;
        criticalDisplay.setText(" Critical: " + criticalCount);
    }

    static void updateAlive() {
        aliveCount = flock.Count-deathCount;
        aliveDisplay.setText(" Alive: " + aliveCount);
    }

    static void lostImmunity() {
        recoveryCount--;
        recoveredDisplay.setText(" Recovered: " + recoveryCount);
        new Sound("immunitylost.wav");
    }

    public void keyReleased( KeyEvent event ) {}

    static List<Boid> addedBoids = new List<>();

    public void keyPressed( KeyEvent event ) {
        //General
        if(event.getKeyCode() == KeyEvent.VK_P)
            Boid.IncrementSeparationMaxForce();
        else if(event.getKeyCode() == KeyEvent.VK_SEMICOLON)
            Boid.DecrementSeparationMaxForce();
        //Toggles
        else if(event.getKeyCode() == KeyEvent.VK_Q)
            toggleCounts(true);
        else if(event.getKeyCode() == KeyEvent.VK_E)
            toggleCounts(false);
        else if(event.getKeyCode() == KeyEvent.VK_W)
            Sound.tickOff = !Sound.tickOff;
        //Sounds
        //Decorative
        else if(event.getKeyCode() == KeyEvent.VK_B)
            new Sound("bell.wav");
        else if(event.getKeyCode() == KeyEvent.VK_N)
            new Sound("ambulance.wav");
        else if(event.getKeyCode() == KeyEvent.VK_SLASH)
            music.stopSong();
        else if(event.getKeyCode() == KeyEvent.VK_PERIOD)
            music = new Sound("plague.wav");
        //Demonstrative
        else if(event.getKeyCode() == KeyEvent.VK_1)
            new Sound("newpatient.wav");
        else if(event.getKeyCode() == KeyEvent.VK_2)
            new Sound("recovery.wav");
        else if(event.getKeyCode() == KeyEvent.VK_3)
            new Sound("immunitylost.wav");
        else if(event.getKeyCode() == KeyEvent.VK_4)
            new Sound("death.wav");
        else if(event.getKeyCode() == KeyEvent.VK_5)
            new Sound("diagnosis.wav");
        else if(event.getKeyCode() == KeyEvent.VK_6)
            new Sound("paranoia.wav");
        else if(event.getKeyCode() == KeyEvent.VK_7)
            new Sound("paranoiaEnded.wav");
        else if(event.getKeyCode() == KeyEvent.VK_8)
            new Sound("treatment.wav");
        else if(event.getKeyCode() == KeyEvent.VK_9)
            new Sound("deathmilestone.wav");
        else if(event.getKeyCode() == KeyEvent.VK_0)
            new Sound("intensity.wav");
        //Demonstations
        else if(event.getKeyCode() == KeyEvent.VK_BACK_SLASH) { //Clear grid
            new Sound("bell.wav");
            clearGrid = true;
        }
        else if(event.getKeyCode() == KeyEvent.VK_R) { //Add healthy
            new Sound("recovery.wav");
            addedBoids.add(new Boid((int)(((UnityEngine.Random.Range(0,1))*WIDTH),(int)(((UnityEngine.Random.Range(0,1))*HEIGHT), false));
        }
        else if(event.getKeyCode() == KeyEvent.VK_F) { //Add infected
            new Sound("recovery.wav");
            addedBoids.add(new Boid((int)(((UnityEngine.Random.Range(0,1))*WIDTH),(int)(((UnityEngine.Random.Range(0,1))*HEIGHT), true));
        }
        else if(event.getKeyCode() == KeyEvent.VK_T) { //Add recovered
            new Sound("recovery.wav");
            Boid recoveredBoid = new Boid((int)((int)(((UnityEngine.Random.Range(0,1))*WIDTH)),(int)(((UnityEngine.Random.Range(0,1))*HEIGHT), false);
            recoveredBoid._isImmune = true;
            recoveredBoid._healthStatus = Boid._recovered;
            recoveredBoid._immunity = recoveredBoid._immunityCap * ((UnityEngine.Random.Range(0,1)*50+100);
            recoveredBoid._immunityCap = recoveredBoid._immunity;
            recoveredBoid._immunityLife = recoveredBoid._initialLifeSpan*(6*((UnityEngine.Random.Range(0,1)*0.8+0.5));
            addedBoids.add(recoveredBoid);
        }
        else if(event.getKeyCode() == KeyEvent.VK_G) { //Add dead
            new Sound("recovery.wav");
            Boid deadBoid = new Boid((int)((int)(((UnityEngine.Random.Range(0,1))*WIDTH)),(int)(((UnityEngine.Random.Range(0,1))*HEIGHT), false);
            deadBoid.IsDead = true;
            deadBoid._healthStatus = Boid.DEAD;
            addedBoids.add(deadBoid);
        }
        else if(event.getKeyCode() == KeyEvent.VK_Y) { //Add paramedic
            new Sound("recovery.wav");
            addedBoids.add(new Boid(true));
        }
        else if(event.getKeyCode() == KeyEvent.VK_H) { //Add diagnosed
            new Sound("recovery.wav");
            new Sound("diagnosis.wav");
            Boid diagnosedBoid = new Boid((int)(((UnityEngine.Random.Range(0,1))*WIDTH),(int)(((UnityEngine.Random.Range(0,1))*HEIGHT), true);
            diagnosedBoid.IsDiagnosed = true;
            diagnosedBoid._healthStatus = diagnosedBoid.DIAGNOSED;
            addedBoids.add(diagnosedBoid);
        }
        else if(event.getKeyCode() == KeyEvent.VK_U) { //Add paranoid
            new Sound("recovery.wav");
            Boid paranoidBoid = new Boid((int)(((UnityEngine.Random.Range(0,1))*WIDTH),(int)(((UnityEngine.Random.Range(0,1))*HEIGHT), false);
            paranoidBoid._healthStatus = Boid._paranoid;
            addedBoids.add(paranoidBoid);
        }

    }
    bool clearGrid = false;


    public void keyTyped(KeyEvent event) {}

    public void mousePressed(MouseEvent event) {
        mouseXPosition = event.getX();   
        mouseYPosition = event.getY();
        addedNewBoid = true;
    }

    //required for compiling; do not use
    public void mouseClicked( MouseEvent event ) {}
    public void mouseReleased( MouseEvent event ) {}
    public void mouseEntered( MouseEvent event ) {}
    public void mouseExited( MouseEvent event ) {}
    // MouseMotionListener: constantly update whenever mouse is moved
    public void mouseMoved(MouseEvent event) {}
    public void mouseDragged(MouseEvent event) {}

}