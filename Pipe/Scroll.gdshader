shader_type canvas_item;

uniform float scroll_speed;
uniform float lod;
varying vec2 shifteduv;
void fragment(){
    vec2 a = UV;
    a.x += TIME * scroll_speed + shifteduv.x;
	shifteduv = a;
    vec4 color = texture(TEXTURE, shifteduv);
    COLOR = color;
}
