////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma once

float _TechnicolorSaturation;
float _TechnicolorColorBalance;

inline float3 FilterTechnicolor(float3 pixel, float2 uv)
{
  float3 red = pixel.r.xxx;
  float3 green = pixel.g.xxx;
  float3 blue = pixel.b.xxx;

  float3 color = float3(
    red.r,
    green.g,
    blue.b
  );

  float3 technicolor = float3(
    pixel.r - (pixel.g + pixel.b) * (_TechnicolorColorBalance * 0.5),
    pixel.g - (pixel.r + pixel.b) * (_TechnicolorColorBalance * 0.5),
    pixel.b - (pixel.r + pixel.g) * (_TechnicolorColorBalance * 0.5)
  );

  technicolor = lerp(pixel, technicolor, _TechnicolorSaturation);

  return clamp(technicolor, 0.0, 1.0);
}
