using UnityEngine;
using System.Collections;

public class SendPositionOnUpdate : MonoBehaviour {

	public OSC osc;
  public OSC oscBinauralVR;
  public OSC binauralizer;
  public float initialY;

	// Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {

	  OscMessage message = new OscMessage();
    message.address = "/SceneRotator/ypr";
    var yaw = transform.rotation.eulerAngles[1];
    var pitch = transform.rotation.eulerAngles[0];
    var roll = transform.rotation.eulerAngles[2];
    if(yaw > 180) yaw = yaw -360.0f;
    if(pitch > 180) pitch = pitch -360.0f;
    if(roll > 180) roll = roll -360.0f;
    Quaternion outputQuat = new Quaternion(transform.rotation.z, transform.rotation.y, transform.rotation.x, -transform.rotation.w);
    message.values.Add(yaw);//YAW
    message.values.Add(-pitch);//PITCH
    message.values.Add(-roll);//ROLL
    // Send Rotation to SceneRotator on port 9000
    osc.Send(message);

    // Send position to CompassBinauralVR on port 9001
    OscMessage offset = new OscMessage();
    offset.address = "/xyzypr";
    offset.values.Add(transform.localPosition.z); //X
    offset.values.Add(-transform.localPosition.x); //Y
    // Debug.Log("Offset position");
    float yOffset = transform.localPosition.y - initialY;
    // Debug.Log(yOffset);
    offset.values.Add(yOffset); //Z
    offset.values.Add(0);//YAW
    offset.values.Add(0);//PITCH
    offset.values.Add(0);//ROLL
    
    // offset.values.Add(-roll);//ROLL
    // offset.values.Add(-pitch);//PITCH
    // offset.values.Add(yaw);//YAW
    
    oscBinauralVR.Send(offset);

     // Send rotation to CompassBinauralVR on port 9002
    // OscMessage rot = new OscMessage();
    // rot.address = "/ypr";
    // rot.values.Add(yaw);//YAW
    // rot.values.Add(-pitch);//PITCH
    // rot.values.Add(-roll);//ROLL
    // binauralizer.Send(rot);
    }

  // public Quaternion MayaRotationToUnity(Vector3 rotation) {
  //  Vector3 flippedRotation = new Vector3(transform.rotation.x, -transform.rotation.y, -transform.rotation.z); // flip Y and Z axis for right->left handed conversion
  //  // convert XYZ to ZYX
  //  Quaternion qx = Quaternion.AngleAxis(flippedRotation.x, Vector3.right);
  //  Quaternion qy = Quaternion.AngleAxis(flippedRotation.y, Vector3.up);
  //  Quaternion qz = Quaternion.AngleAxis(flippedRotation.z, Vector3.forward);
  //  Quaternion qq = qz * qy * qx ; // this is the order
  //  return qq;
  // }



}
