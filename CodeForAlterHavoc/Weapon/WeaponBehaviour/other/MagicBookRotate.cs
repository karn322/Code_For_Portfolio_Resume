using UnityEngine;

public class MagicBookRotate : MonoBehaviour
{
    public MagicBookBehaviour[] _MagicBookBehaviour;
    public float _Speed = 100f;

    private void Update()
    {
        if (_MagicBookBehaviour[0] == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.Rotate(new Vector3(0, 0, _Speed) * Time.deltaTime);
        for (int i = 0; i < _MagicBookBehaviour.Length; i++)
        {
            _MagicBookBehaviour[i].RotateThis(_Speed);
        }

        if(transform.rotation.x == 0)
        {
            for (int i = 0; i < _MagicBookBehaviour.Length; i++)
            {
                _MagicBookBehaviour[i]._EnemyMark.Clear();
            }
        }
        
    }
}
