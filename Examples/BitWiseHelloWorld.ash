##: Bitwise Hello World
##: Load the chars into the buffer
TOCHAR : ba 104
TOCHAR : bb 101
TOCHAR : bc 108
TOCHAR : bd 108
TOCHAR : be 111
TOCHAR : bf 95
TOCHAR : bg 119
TOCHAR : bh 111
TOCHAR : bi 114
TOCHAR : bj 108
TOCHAR : bk 100
TOCHAR : bl 33
##: Concat the the values together
CONCAT : aa ba bb
CONCAT : aa aa bc
CONCAT : aa aa bd
CONCAT : aa aa be
CONCAT : aa aa bf
CONCAT : aa aa bg
CONCAT : aa aa bh
CONCAT : aa aa bi
CONCAT : aa aa bj
CONCAT : aa aa bk
CONCAT : aa aa bl
##: All chars were added to the buffer
PRINT : aa
EXIT : 0
