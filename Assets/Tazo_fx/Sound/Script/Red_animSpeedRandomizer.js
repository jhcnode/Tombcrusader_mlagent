
var minSpeed:float=0.7;
var maxSpeed:float=1.5;

function Start () {
   // animation[animation.clip.name].speed = Random.Range(minSpeed, maxSpeed);
    ((GetComponent.<Animation>() as Animation)[GetComponent.<Animation>().clip.name] as AnimationClip).speed=speed=Random.Range(minSpeed, maxSpeed);
}