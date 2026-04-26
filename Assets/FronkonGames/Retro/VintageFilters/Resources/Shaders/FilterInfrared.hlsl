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

float _InfraredFoliage;
float _InfraredBloom;

inline float3 FilterInfrared(float3 pixel, float2 uv)
{
  // Infrared effect: greens/yellows become bright white/pink, blues become dark.
  float luma = dot(pixel, float3(0.299, 0.587, 0.114));
  
  float greeness = max(0.0, pixel.g - max(pixel.r, pixel.b));
  
  float3 ir = float3(luma, luma * 0.8, luma * 0.9);
  ir += greeness * _InfraredFoliage * float3(1.0, 0.5, 0.6);
  
  // Bloom approximation.
  float3 bloom = pow(max(0.0, ir - 0.5), 2.0) * _InfraredBloom;
  ir += bloom;

  return clamp(ir, 0.0, 1.0);
}
