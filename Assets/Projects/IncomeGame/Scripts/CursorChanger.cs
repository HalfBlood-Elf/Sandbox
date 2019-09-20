using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour {
    public Vector2 cursorHotspot = new Vector2(4, 3);

    [SerializeField]
    private List<Texture2D> sprites;
    public CursorChanger instance = null;

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(sprites[0], cursorHotspot, CursorMode.Auto);
    }
    public void SetActiveCursor()
    {
        Cursor.SetCursor(sprites[1], cursorHotspot, CursorMode.Auto);
    }

    
    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
            Destroy(gameObject);
        Application.runInBackground = true;
    }
}
