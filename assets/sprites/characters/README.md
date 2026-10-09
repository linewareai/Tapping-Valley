# ValleyTapping pet sprites

## Lillia
- Internal ID: `lillia`
- Species: dog
- Visual identity: scruffy cream/apricot coat, tousled head fur, floppy ears, dark eyes and nose.
- **No collar, leash, harness, or clothing**, per art direction.
- Current sprite is an initial in-game placeholder source, not the finished animation sheet. The generated reference sheet should be sliced into separately validated transparent PNG frames before being used as a frame atlas.

## Pet system conventions
- Internal IDs are not player-facing names. Store custom display names separately.
- Pet availability is gated by game progression; adding an asset must not force it to spawn.
- Active pet is saved in the local save data.
- Sprite URLs should be switched through the pet registry, not hard-coded across views.
