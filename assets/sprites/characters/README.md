# Pet character source sprites

This directory contains the first detailed pixel-art-style source sprites for the reference pets.

## Internal IDs and art

- `lillia` (dog): warm cream/apricot coat, tousled fur, floppy ears, dark nose; reference includes a pink harness.
- `teemo` (cat): gray-and-white longhair, white chest and face blaze, pink nose, fluffy tail.
- `ahri` (cat): golden-brown tabby, dark stripes, green eyes, ringed tail.
- `campi` (cat): existing first cat reference; gray-and-white coat and purple collar.

## Asset conventions

- Individual transparent SVG files, each on a 24 × 24 logical pixel grid rendered at 96 × 96.
- Crisp-edged geometry and clustered shading are intentional; do not smooth or apply blur filters.
- Internal IDs are stable technical identifiers, not player-facing names. Player display names must be stored separately and customizable.
- Species and appearance are separate from unlock/drop rules. No availability, rarity, or unlock behavior is implied by adding art assets.
- The current files are initial character-design passes, not a completed animation set. Build idle/walk/react/sleep/celebrate frames only after reviewing and refining the master appearance at actual game scale.
- When creating animation frames, preserve the same palette, markings, eye placement, head/body proportions, and silhouette for each character.
