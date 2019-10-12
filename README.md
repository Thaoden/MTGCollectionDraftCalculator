# MTGDraftCollectionCalculator
This calculator attempts to estimate the number of BO1 drafts needed to complete one's rare collection in MtG Arena.

The problem of collecting a collection of cards is known as the coupon collector's problem and is discussed, among others, here:

- https://probabilityandstats.wordpress.com/2017/01/18/how-long-does-it-take-to-collect-all-coupons/
- https://www.mathpages.com/home/kmath437.htm
- https://math.stackexchange.com/questions/2315285/coupon-collectors-problem-with-x-amount-of-coupons-already-collected

The first version makes some very naive assumptions:
* Assume there is a single set of cards (when in reality, 4 sets of all cards can be collected)
* Do not use wildcards
* Do not take into account mythics
* Every draft pack has exactly one rare
* Do not consider buying prizes
  * By expansion, do not care about gems or coins
* Do not take into account the packs won by finishing a draft
* Do not consider gem rewards
* Do not consider gem earned if a rare or mythic already collected is picked

For a later version, in addition to the points above, mythics and wildcards in draft packs and booster packs will be considered. The following probabilities will be used:

Each "regular" booster pack contains approximately:
* 0.83 rares, 0.18 rare WC (in pack + track)
* 0.08 mythics, 0.08 mythic WC  (in pack + track)

Each draft pack contains approximately:
* 0.xx rares
* 0.xx mythics

Additionally, there is a chance of xx% for an additional rare on the second pick of a draft (value determined empirically courtesy of MTGA Tool).

Not all cards from the collection can be drafted, some rares have to be created by using wildcards. In ELD, there are 276 rares in total, 212 in draft packs (4x 69; 4x 53).
