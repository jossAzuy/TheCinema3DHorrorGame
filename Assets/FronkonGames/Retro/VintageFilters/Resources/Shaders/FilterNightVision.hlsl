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

float _NightVisionGain;
float _NightVisionNoise;

inline float3 FilterNightVision(float3 pixel, float2 uv)
{
  float gray = dot(pixel, float3(0.299, 0.587, 0.114));
  gray *= (1.0 + _NightVisionGain);
  
  float3 green = float3(gray * 0.1, gray, gray * 0.1);
  
  // Scanlines.
  float scanline = sin(uv.y * 500.0) * 0.04;
  green -= scanline;
  
  // Static noise.
  float noise = (frac(sin(dot(uv + _EffectTime.y, float2(12.9898, 78.233))) * 43758.5453) - 0.5) * _NightVisionNoise;
  green += noise;

  return clamp(green, 0.0, 1.0);
}
