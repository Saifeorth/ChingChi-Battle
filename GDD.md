# ChingChi Battle — Game Design Document
**Version:** 0.1 (Pre-Production)
**Target Platform:** Android (Google Play Store)
**Genre:** Arcade Car Combat — Casual
**Engine:** Unity 6.3 LTS (6000.3.18f1)

---

## 1. Vision Statement

**ChingChi Battle** is a casual arcade vehicle combat game set in a vibrant Pakistani street-art universe. Players drive iconic desi transport vehicles — rickshaws, Qingqi (chingchi), painted trucks, and tuk-tuks — loaded with wild weapons, and battle other players or AI across cartoon-style maps inspired by Pakistani cities, bazaars, and highways. Think *Road Rash* meets *Twisted Metal* meets *Pakistani truck art*.

---

## 2. Core Pillars

| Pillar | Description |
|---|---|
| **Desi Soul** | Art, audio, maps, and humor rooted in Pakistani culture |
| **Pick Up & Play** | 2–3 minute matches, simple controls, instant fun |
| **Weapon Mayhem** | Absurd weapon combinations on everyday vehicles |
| **Progression Loop** | Unlock vehicles, weapons, and cosmetics |

---

## 3. Unique Selling Points (USP)

- First arcade vehicle combat game with authentic Pakistani truck-art aesthetics
- Iconic desi vehicles (chingchi, rikshaw, truck, suzuki carry, bus) as playable characters
- Vibrant hand-painted truck-art visual style applied to UI and vehicles
- Local cultural humor (vehicle names, honking, Urdu/Punjabi exclamations as SFX)
- Offline single-player + optional online PvP

---

## 4. Target Audience

- **Primary:** Pakistani mobile gamers, 13–28, casual to mid-core
- **Secondary:** South Asian diaspora globally
- **Tertiary:** Global players attracted by unique art style

---

## 5. Vehicle Roster (Initial)

| Vehicle | Class | Flavor |
|---|---|---|
| Chingchi (Qingqi) | Light / Fast | 3-wheel open rickshaw, fragile but nimble |
| Rikshaw (CNG) | Light / Medium | Classic yellow-black, balanced |
| Suzuki Carry | Medium | Mini pickup truck, moderate armor |
| Decorated Truck | Heavy / Slow | Full Pakistani truck art, high HP, slow turn |
| Mini Bus | Heavy | Colorful Lahori/Karachi bus, rams well |
| Tractor Trolley | Tank | Rural Pakistan, extreme HP, barely steers |

Each vehicle has:
- **HP** (health pool)
- **Speed** stat
- **Armor** stat
- **Weapon Slots** (1–3 depending on class)

---

## 6. Weapon System

Weapons mount on vehicle slots. Each vehicle class limits which weapons can be equipped.

### Weapon Categories

| Category | Examples |
|---|---|
| **Projectile** | Potato cannon, Cricket ball launcher, Matka (clay pot) mortar |
| **Melee** | Spinning danda (stick), Chain flail, Shovel |
| **Area** | Mirchi (chili) gas cloud, Smoke paan spit, Oil slick |
| **Special** | Honk Blast (sonic), Dhol (drum) shockwave, Jugaad rocket |

### Weapon Rarity
Common → Rare → Epic → Legendary (affects damage, visual FX, and unlock method)

---

## 7. Game Modes

### Phase 1 (MVP)
- **Solo Campaign** — Wave-based AI battles across 5 maps. 3 difficulty tiers.
- **Quick Match (Local)** — Split-screen or bot skirmish, 3-minute deathmatch

### Phase 2
- **Online PvP** — 2v2 or 3v3 team deathmatch via Photon (existing integration)
- **Convoy Mode** — Escort/attack a cargo truck across a map

### Phase 3
- **Battle Royale Lite** — 8 players, shrinking zone, last vehicle standing
- **Ranked Season** — ELO-based ranked PvP with seasonal cosmetic rewards

---

## 8. Map Designs

| Map | Setting | Vibe |
|---|---|---|
