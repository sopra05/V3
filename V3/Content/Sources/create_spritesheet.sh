#!/bin/bash
rows=8      # Possible directions (== 8 in isometric view)
columns=32  # Number of sprites per direction
cd out
montage *.png -mode concatenate -tile ${columns}x${rows} -background none out.png
mv out.png ../
cd ..
