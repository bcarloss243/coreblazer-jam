// Assets/Scripts/PlayerMover.cs
using UnityEngine;
public class PlayerMover : MonoBehaviour {
  public float speed = 3f;
  void Update() {
    var v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    transform.Translate(v * speed * Time.deltaTime);
  }
}
