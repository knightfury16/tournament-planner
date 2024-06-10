# SVG

## Position 

Svg support `absolute unit` one with simensional identifier like 'pt' or 'cm' and `user units` without any dimensional identifier which are plain simple numbers.

If we dont specify anything, one `user unit` equal to one screen unit, like pixel.


We can explicitly define what one `user unit` mean.

```html
<svg width="100" height="100">…</svg>
```

The above code define a svg canvas with **100x100px**. Where one `user unit` equals one screen unit.

```html
<svg width="200" height="200" viewBox="0 0 100 100">…</svg>
```

Here the whole canvas in **200x200px**. But here the `viewbox` attribute define the portion of the canvas to **display**.

As the name suggest viewbox is like a cutout of box through which you view the canvas.

The current mapping of user units to screen units is called **user coordinate system**
.

Apart form scaling the coordinate system can also be *rotated*, *skewed*, and *flipped*
The so called **Geometric Transformation**


The default user coordinate system maps one user pixel to one device pixel.

`1 user pixel = 1 pixel`, *default*.

But we can also specify our own measurment. Like `1in` or `1cm`. In that case system calculate what `1in` in pixel count means.

> [...] suppose that the user agent can determine from its environment that "1px" corresponds to "0.2822222mm" (i.e., 90dpi). Then, for all processing of SVG content: [...] "1cm" equals "35.43307px" (and therefore 35.43307 user units)


## Basic Shapes

### Rectangle

The `<rect>` element draws a rectangle on the screen. There are six basic attributes that control the position and shape of the rectangle on screen. 

```html
<rect x="10" y="10" width="30" height="30"/>
<rect x="60" y="10" rx="10" ry="10" width="30" height="30"/>
```
**`x`**

The x position of the top left corner of the rectangle.

**`y`**

The y position of the top left corner of the rectangle.

**`width`**

The width of the rectangle.

**`height`**

The height of the rectangle.

**`rx`**

The x radius of the corners of the rectangle.

**`ry`**

The y radius of the corners of the rectangle.


### Circle

`<circle>` draws circle. It takes basic three parameters to draw the circle.

**`r`**

The radius of the circle.

**`cx`**

The x position of the center of the circle.

**`cy`**

The y position of the center of the circle.


### Ellipse

An `<ellipse>` is a more general form of the <circle> element, where you can scale the x and y radius (commonly referred to as the semimajor and semiminor axes in maths) of the circle separately.

```html
<ellipse cx="75" cy="75" rx="20" ry="5"/>
```

**`rx`**

The x radius of the ellipse.

**`ry`**

The y radius of the ellipse.

**`cx`**

The x position of the center of the ellipse.

**`cy`**

The y position of the center of the ellipse.

### Line

The `<line>` element takes the positions of two points as parameters and draws a straight line between them.

```html
<line x1="10" x2="50" y1="110" y2="150" stroke="black" stroke-width="5"/>
```

**`x1`**

The x position of point 1.

**`y1`**

The y position of point 1.

**`x2`**

The x position of point 2.

**`y2`**

The y position of point 2.

### Polyline

A <polyline> is a group of connected straight lines. Since the list of points can get quite long, all the points are included in one attribute:
```html
<polyline points="60, 110 65, 120 70, 115 75, 130 80, 125 85, 140 90, 135 95, 150 100, 145"/>
```
`points`

A list of points. Each number must be separated by a space, comma, EOL, or a line feed character with additional whitespace permitted. Each point must contain two numbers: an x coordinate and a y coordinate. So, the list (0,0), (1,1), and (2,2) could be written as 0, 0 1, 1 2, 2.

### Polygon

A `<polygon>` is similar to a <polyline>, in that it is composed of straight line segments connecting a list of points. For polygons though, the path automatically connects the last point with the first, creating a closed shape.

> Note: A rectangle is a type of polygon, so a polygon can be used to create a <rect/> element that does not have rounded corners.

```html
<polygon points="50, 160 55, 180 70, 180 60, 190 65, 205 50, 195 35, 205 40, 190 30, 180 45, 180"/>
```
**`points`**

A list of points, each number separated by a space, comma, EOL, or a line feed character with additional whitespace permitted. Each point must contain two numbers: an x coordinate and a y coordinate. So, the list (0,0), (1,1), and (2,2) could be written as 0, 0 1, 1 2, 2. The drawing then closes the path, so a final straight line would be drawn from (2,2) to (0,0)
