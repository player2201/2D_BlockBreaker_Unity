using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //config params
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    //[SerializeField] int maxHits;                 REMOVED FOR BETTER LOGIC OF maxHITS
    [SerializeField] Sprite[] hitSprites;
   
    //state variables
    [SerializeField] int timesHit; //only for debug
   
    //cached reference
    Level level;
   
    //count the number of blocks
    private void Start()
    {
        CountBreakableBlocks();
    }
    
    //method for counting breakable blocks
    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    //When ball touches the block
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHit();
        }
    }
   
    //method for destroying blocks after a number of hits
    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlocks();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        //for the array to be 0, when the bal hits the first time we minus 1 so that it becomes 0
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block sprite is missing from array" + gameObject.name);
        }
    }

    private void DestroyBlocks()
    {
        PlayDestroyBlockSFX();
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerSparklesVFX();

    }
    
    //method for destroying blocks SFX
    private void PlayDestroyBlockSFX()
    {
        FindObjectOfType<GameSession>().AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    //to trigger the sparkle VFX
    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }

}
