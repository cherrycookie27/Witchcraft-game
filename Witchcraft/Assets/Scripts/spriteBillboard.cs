using UnityEngine;

public class spriteBillboard : MonoBehaviour
{

    [SerializeField] bool ffreezeXZAxis = true;





    // Update is called once per frame
    void Update()
    {
        if (ffreezeXZAxis)
        {
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }


    }
}
