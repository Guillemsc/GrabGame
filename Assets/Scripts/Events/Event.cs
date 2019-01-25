using UnityEngine;

public enum EventType
{
    EVENT_NULL,

    EVENT_PLAYER_START_TOUCHING_WALL,
    EVENT_PLAYER_STOP_TOUCHING_WALL,
    EVENT_PLAYER_START_TOUCHING_PLATFORM,
    EVENT_PLAYER_STOP_TOUCHIING_PLATFORM,
    EVENT_PLAYER_WEAPON_START_GRAB,

    EVENT_PLAYER_DIES,
    EVENT_PLAYER_REESPAWNS,

    EVENT_LEVEL_START,
    EVENT_LEVEL_LOAD,

    EVENT_LEVEL_END,
    EVENT_LEVEL_UNLOAD,
}

public class Event
{
    public Event(EventType e_type)
    {
        event_type = e_type;
    }

    public EventType Type()
    {
        return event_type;
    }

    // Player walls and platforms
    public class PlayerStartTouchingWall
    {
        public GameObject wall = null;
    }
    public PlayerStartTouchingWall player_start_touching_wall = new PlayerStartTouchingWall();

    public class PlayerStopTouchingWall
    {
        public GameObject wall = null;
    }
    public PlayerStopTouchingWall player_stop_touching_wall = new PlayerStopTouchingWall();

    public class PlayerStartTouchingPlatform
    {
        public GameObject platform = null;
    }
    public PlayerStartTouchingPlatform player_start_touching_platform = new PlayerStartTouchingPlatform();

    public class PlayerStopTouchingPlatform
    {
        public GameObject platform = null;
    }
    public PlayerStopTouchingPlatform player_stop_touching_platform = new PlayerStopTouchingPlatform();

    public class PlayerWeaponStartGrab
    {
        public GameObject grabed_point = null;
    }
    public PlayerWeaponStartGrab player_weapon_start_grab = new PlayerWeaponStartGrab();

    // Players
    public class PlayerDies
    {
        public GameObject player = null;
    }
    public PlayerDies player_dies = new PlayerDies();

    public class PlayerReespawns
    {
        public GameObject player = null;
    }
    public PlayerReespawns player_reespawns = new PlayerReespawns();


    // Levels
    public class LevelStart
    {
        public Level starting_level = null;
    }
    public LevelStart level_start = new LevelStart();

    public class LevelLoad
    {
        public Level to_load = null;
        public LevelConnection spawning_connection = null;
    }
    public LevelLoad level_load = new LevelLoad();

    public class LevelEnd
    {
        public Level ending_level = null;
    }
    public LevelEnd level_end = new LevelEnd();

    public class LevelUnload
    {
        public Level to_unload = null;
    }
    public LevelUnload level_unload = new LevelUnload();

    private EventType event_type = EventType.EVENT_NULL;
}

