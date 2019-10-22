# MTGDraftCollectionCalculator

This calculator attempts to estimate the number of BO1 drafts needed to complete one's rare collection in MtG Arena.

To do so, a draft is simulated by opening three packs, assuming only the first pick presents a rare. During a draft, as there is no duplicate protection, this rare can be any rare of the complete (draftable) set. A random card is created and checked against the current collection if it is a duplicate. If this is not the case, it is added the collection. After each draft, the amount of boosters owned is incremented by one.
As soon as the owned boosters exceed the amount of missing cards, the estimation stops.

The current version currently only considers the "Throne of Eldraine" set and makes some very naive assumptions:
* Do not use wildcards
* Do not take into account mythics
* Every draft pack has exactly one rare
* Do not consider buying prizes
  * By expansion, do not care about gems or coins
* Do not take into account the potential additional pack won by finishing a draft (20% for 0 wins, 100% for 7 wins)
* Do not consider gem rewards
* Do not consider gems earned if a rare or mythic already collected is picked
* Do not consider possible booster packs remaining from mastery path
  * By expansion, do not consider if the user purchased the mastery pass

For a later version, in addition to the points above, mythics and wildcards in draft packs and booster packs will be considered. The following probabilities will be used:

Each "regular" booster pack contains approximately:
* 0.83 rares, 0.18 rare WC (in pack + track)
* 0.08 mythics, 0.08 mythic WC  (in pack + track)

Each draft pack contains approximately:
* 0.xx rares
* 0.xx mythics

Additionally, there is a chance of xx% for an additional rare on the second pick of a draft (value determined empirically courtesy of MTGA Tool).

A later version will also try to recommend where to spend the currencies, drafting or buying packs, based on the current collection state.

Not all cards from the collection can be drafted, some rares have to be created by using wildcards. In ELD, there are 276 rares in total, 212 in draft packs (4x 69; 4x 53).


# Thanks
This project is powered by [MTGATool](https://mtgatool.com/). Big thanks for providing me with their data!
