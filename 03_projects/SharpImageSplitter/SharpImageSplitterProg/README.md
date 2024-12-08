Strategies description
 - ??
 - ??

HeightInfo Explanation

   ^  _________ <= 0
   |  |  10   |
   |  _________ <= 10
   |  |       |
70 |  |  40   |
   |  |       |
   |  _________ <= 50
   |  |  20   |
   V  _________ <= 70

          ^  ___________________ <= StartOfTopHeight
          |  | TopHeightLen    |
          |  ___________________ <= StartOfMiddleHeight
          |  |                 |
Rectangle |  | MiddleHeightLen |
  Height  |  |                 |
          |  ___________________ <= StartOfBottomHeight
          |  | BottomHeightLen |
          V  ___________________ <= EndHeight

// wyjaśnienie
// kazdy srodkowy obrazek ma MiddleHeightLen + 2*strap
// pierwszy obrazek ma MiddleHeightLen + 1*strap 
// ostatni obrazek ma 1*strap + smallHeight które jest mniejsze

//What TopOffset change will affect?
FirstMiddleHeight
ImagesCount

Rectangle [ X=0, Y=0, Width=1744, Height=2570 ]
Rectangle [ X=0, Y=2495, Width=1744, Height=2571 ]
Rectangle [ X=0, Y=7485, Width=1744, Height=1711 ]

Y_T - Y Top - Start Y Position of Top Height
Y_M - Y Middle - Start Y Position of Middle Height
Y_B - Y Bottom Position
Y_E - Y End (Height)
S_T - Strap Top
S_B - Strap Bottom
H_C - Height Crop - Rectangle  (of whole crop rectangle)
H_M - Height Middle -

Y_T - YTopStartPosOfTopHeight
Y_M - YMiddleStartPosOfMiddleHeight
Y_B - YBottom_StartPosOfBottomHeight
Y_E - YEnd_EndPosOfBottomHeight

HInfo [ Y_T=0, Y_M=0, Y_B=2545, Y_E=2570, S_T=0, S_B=25, H_C=2570], H_M=2545]
HInfo [ Y_T=2495, Y_M=2520, Y_B=5041, Y_E=5066, S_T=25, S_B=25, H_C=2571], H_M=2521]
HInfo [ Y_T=4990, Y_M=5015, Y_B=7536, Y_E=7561, S_T=25, S_B=25, H_C=2571], H_M=2521]
HInfo [ Y_T=7485, Y_M=1661, Y_B=9171, Y_E=9196, S_T=-5824, S_B=25, H_C=1711], H_M=7510]

YST - Y Start Top (Height)
YSM - Y Start Middle (Height)
YSM - Y Start Bottom (Height)
YE - Y End (Height)
TS - Top Strap
BS - Bottom Strap
RH - Rectangle Height (of whole crop rectangle)
MH - Middle Height (len)

HInfo [ YST=0, YSM=200, YSM=2295, YE=2495, TS=200, BS=200, RH=2570]
