# Reagent Loop 

## 
Mecanique de 4 Afflictions

`Hunger`

`Bleeding`

`Curse`

`Poison`

Les items ont souvent un effet positif (`cure`), et un effet négatif (`inflict`). 

`cure` bascule l'état `IsCured` à `true`

`inflict` bascule l'état `IsCured` à `false`

## États spéciaux (prévention, persistence)

Les `affliction` peuvent etre soit en état `persistent` ou en état `prevented`.

La `prevention` a pour effet que l'affliction est protégée d'être `inflicted` une (1) fois.

La `persistence` a pour effet qu'une `cure` de l'`affliction` est ineffective pour une (1) `cure`. 

## Conditions de win

On Gagne quand les 4 états sont à `IsCured = true`

## Condition de mort.

On Perd quand durant 3 rounds (non consécutives), toutes les afflictions sont actives en même temps. 

Cela devrait arriver rarement, surtout lorsque le joueur donne consécutivement plusieurs reagents très négatifs qui donne par exemple plusieurs `persistence` sur des afflictions diff

## Idées

Frapper une piege, out te faire hit par un chat, etc. ca rend l'item moins bon (gagne une persistence, peut gagner )